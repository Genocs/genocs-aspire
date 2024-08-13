using GenocsAspire.SignalRApiService.Messages.Events;

namespace GenocsAspire.SignalRApiService.Services;

public interface IHubService
{
    Task PublishOperationPendingAsync(OperationPending @event);
    Task PublishOperationCompletedAsync(OperationCompleted @event);
    Task PublishOperationRejectedAsync(OperationRejected @event);
}