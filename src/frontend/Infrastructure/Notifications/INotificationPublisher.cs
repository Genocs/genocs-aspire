using Genocs.BlazorAspire.Shared.Notifications;

namespace Genocs.BlazorAspire.Infrastructure.Notifications;

public interface INotificationPublisher
{
    Task PublishAsync(INotificationMessage notification);
}