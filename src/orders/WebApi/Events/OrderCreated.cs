using Genocs.Core.CQRS.Events;

namespace GenocsAspire.OrdersApiService.Events;

public class OrderCreated : IEvent
{
    public Guid OrderId { get; }

    public OrderCreated(Guid orderId)
    {
        OrderId = orderId;
    }
}