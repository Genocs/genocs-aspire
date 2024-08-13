using Genocs.Core.CQRS.Commands;
using Genocs.MessageBrokers;
using Genocs.MessageBrokers.Outbox;
using Genocs.Persistence.MongoDb.Repositories.Mentor;
using GenocsAspire.ProductsApiService.Domain;
using GenocsAspire.ProductsApiService.Events;
using OpenTracing;

namespace GenocsAspire.ProductsApiService.Commands.Handlers;

public class CreateProductHandler : ICommandHandler<CreateProduct>
{
    private readonly IMongoRepository<Product, Guid> _repository;
    private readonly IBusPublisher _publisher;
    private readonly IMessageOutbox _outbox;
    private readonly ILogger<CreateProductHandler> _logger;
    private readonly ITracer _tracer;

    public CreateProductHandler(
                                IMongoRepository<Product, Guid> repository,
                                IBusPublisher publisher,
                                IMessageOutbox outbox,
                                ITracer tracer,
                                ILogger<CreateProductHandler> logger)
    {
        _repository = repository;
        _publisher = publisher;
        _outbox = outbox;
        _tracer = tracer;
        _logger = logger;
    }

    public async Task HandleAsync(CreateProduct command, CancellationToken cancellationToken = default)
    {
        bool exists = await _repository.ExistsAsync(o => o.Id == command.ProductId);
        if (exists)
        {
            throw new InvalidOperationException($"Product with given id: {command.ProductId} already exists!");
        }

        var product = new Product(command.ProductId, command.SKU, command.UnitPrice);
        await _repository.AddAsync(product);

        _logger.LogInformation($"Created a product with id: {command.ProductId}, sku: {command.SKU}, unitPrice: {command.UnitPrice}.");

        string? spanContext = _tracer.ActiveSpan?.Context.ToString();
        var @event = new ProductCreated(product.Id);
        if (_outbox.Enabled)
        {
            await _outbox.SendAsync(@event, spanContext: spanContext);
            return;
        }

        await _publisher.PublishAsync(@event, spanContext: spanContext);
    }
}