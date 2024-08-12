using Genocs.Core.CQRS.Events;
using Genocs.MessageBrokers;

namespace GenocsAspire.OrdersApiService.Events.External;

[Message("deliveries")]
public class DeliveryStarted : IEvent
{
    public Guid DeliveryId { get; }

    public DeliveryStarted(Guid deliveryId)
    {
        DeliveryId = deliveryId;
    }
}