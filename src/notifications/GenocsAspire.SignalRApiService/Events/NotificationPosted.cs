namespace GenocsAspire.SignalRApiService.Events;

public class NotificationPosted
{

    public Guid NotificationId { get; }

    public NotificationPosted(Guid notificationId)
    {
        NotificationId = notificationId;
    }
}
