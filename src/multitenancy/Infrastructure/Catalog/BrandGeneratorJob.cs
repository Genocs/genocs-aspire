﻿using Ardalis.Specification;
using Genocs.MultitenancyAspire.Application.Catalog.Brands;
using Genocs.MultitenancyAspire.Application.Common.Interfaces;
using Genocs.MultitenancyAspire.Application.Common.Persistence;
using Genocs.MultitenancyAspire.Domain.Catalog;
using Hangfire;
using Hangfire.Console.Extensions;
using Hangfire.Console.Progress;
using Hangfire.Server;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Genocs.MultitenancyAspire.Infrastructure.Catalog;

public class BrandGeneratorJob : IBrandGeneratorJob
{
    private readonly ILogger<BrandGeneratorJob> _logger;
    private readonly ISender _mediator;
    private readonly IReadRepository<Brand> _repository;
    private readonly IProgressBarFactory _progressBar;
    private readonly PerformingContext _performingContext;
    private readonly INotificationSender _notifications;
    private readonly ICurrentUser _currentUser;
    private readonly IProgressBar _progress;

    public BrandGeneratorJob(
                            ILogger<BrandGeneratorJob> logger,
                            ISender mediator,
                            IReadRepository<Brand> repository,
                            IProgressBarFactory progressBar,
                            PerformingContext performingContext,
                            INotificationSender notifications,
                            ICurrentUser currentUser)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _progressBar = progressBar ?? throw new ArgumentNullException(nameof(progressBar));
        _performingContext = performingContext ?? throw new ArgumentNullException(nameof(performingContext));
        _notifications = notifications ?? throw new ArgumentNullException(nameof(notifications));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _progress = _progressBar.Create();
    }

    private async Task NotifyAsync(string message, int progress, CancellationToken cancellationToken)
    {
        _progress.SetValue(progress);
        await _notifications.SendToUserAsync(
            new JobNotification()
            {
                JobId = _performingContext.BackgroundJob.Id,
                Message = message,
                Progress = progress
            },
            _currentUser.GetUserId().ToString(),
            cancellationToken);
    }

    [Queue("notdefault")]
    public async Task GenerateAsync(int nSeed, CancellationToken cancellationToken)
    {
        await NotifyAsync("Your job processing has started", 0, cancellationToken);

        foreach (int index in Enumerable.Range(1, nSeed))
        {
            await _mediator.Send(
                new CreateBrandRequest
                {
                    Name = $"Brand Random - {DefaultIdType.NewGuid()}",
                    Description = "Funny description"
                },
                cancellationToken);

            await NotifyAsync("Progress: ", nSeed > 0 ? index * 100 / nSeed : 0, cancellationToken);
        }

        await NotifyAsync("Job successfully completed", 0, cancellationToken);
    }

    [Queue("notdefault")]
    [AutomaticRetry(Attempts = 5)]
    public async Task CleanAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Initializing Job with Id: {jobId}", _performingContext.BackgroundJob.Id);

        var items = await _repository.ListAsync(new RandomBrandsSpec(), cancellationToken);

        _logger.LogInformation("Brands Random: {brandsCount} ", items.Count.ToString());

        foreach (var item in items)
        {
            await _mediator.Send(new DeleteBrandRequest(item.Id), cancellationToken);
        }

        _logger.LogInformation("All random brands deleted.");
    }
}

public class RandomBrandsSpec : Specification<Brand>
{
    public RandomBrandsSpec()
        => Query.Where(b => !string.IsNullOrEmpty(b.Name) && b.Name.Contains("Brand Random"));
}