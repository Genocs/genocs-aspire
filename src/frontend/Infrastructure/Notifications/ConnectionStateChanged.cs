using Genocs.BlazorAspire.Shared.Notifications;

namespace Genocs.BlazorAspire.Infrastructure.Notifications;

public record ConnectionStateChanged(ConnectionState State, string? Message) : INotificationMessage;