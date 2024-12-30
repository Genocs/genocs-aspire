using Genocs.MultitenancyAspire.Shared.Events;

namespace Genocs.MultitenancyAspire.Application.Common.Events;

public class EventNotification<TEvent> : INotification
    where TEvent : IEvent
{
    public EventNotification(TEvent @event) => Event = @event;

    public TEvent Event { get; }
}