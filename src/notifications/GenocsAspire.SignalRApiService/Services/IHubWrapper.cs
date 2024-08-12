namespace GenocsAspire.SignalRApiService.Services;

public interface IHubWrapper
{
    Task PublishToUserAsync(Guid userId, string message, object data);
    Task PublishToAllAsync(string message, object data);
}