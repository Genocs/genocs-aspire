using Genocs.Core.CQRS.Commands;
using GenocsAspire.Identities.Application.Domain.Entities;
using GenocsAspire.Identities.Application.Domain.Exceptions;
using GenocsAspire.Identities.Application.Domain.Repositories;
using GenocsAspire.Identities.Application.Events;
using GenocsAspire.Identities.Application.Services;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace GenocsAspire.Identities.Application.Commands.Handlers;

internal sealed class CreateUserHandler : ICommandHandler<CreateUser>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IMessageBroker _messageBroker;
    private readonly ILogger<CreateUserHandler> _logger;

    private static readonly Regex EmailRegex = new Regex(
        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
        RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.CultureInvariant);

    public CreateUserHandler(IUserRepository userRepository, IPasswordService passwordService,
        IMessageBroker messageBroker, ILogger<CreateUserHandler> logger)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _messageBroker = messageBroker;
        _logger = logger;
    }

    public async Task HandleAsync(CreateUser command, CancellationToken cancellationToken = default)
    {
        if (!EmailRegex.IsMatch(command.Email))
        {
            _logger.LogError($"Invalid email: {command.Email}");
            throw new InvalidEmailException(command.Email);
        }

        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user is { })
        {
            _logger.LogError($"Email already in use: {command.Email}");
            throw new EmailInUseException(command.Email);
        }

        user = await _userRepository.GetByNameAsync(command.Name);
        if (user is { })
        {
            _logger.LogError($"Name already in use: {command.Name}");
            throw new NameInUseException(command.Name);
        }

        var role = string.IsNullOrWhiteSpace(command.Role) ? "user" : command.Role.ToLowerInvariant();
        var password = _passwordService.Hash(command.Password);
        user = new User(command.UserId, command.Email, command.Name, password, role, DateTime.UtcNow,
            command.Permissions);
        await _userRepository.AddAsync(user);
        _logger.LogInformation($"Created an account for the user with ID: '{user.Id}'.");
        await _messageBroker.PublishAsync(new UserCreated(user.Id, user.Name, user.Role));
    }
}