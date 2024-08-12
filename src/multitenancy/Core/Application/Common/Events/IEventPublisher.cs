using GenocsAspire.Multitenancy.Application.Common.Interfaces;
using GenocsAspire.Multitenancy.Shared.Events;

namespace GenocsAspire.Multitenancy.Application.Common.Events;

/// <summary>
/// The event publisher interface.
/// </summary>
public interface IEventPublisher : ITransientService
{
    /// <summary>
    /// Publish the event.
    /// </summary>
    /// <param name="event">The event to be publish.</param>
    /// <returns>The task.</returns>
    Task PublishAsync(IEvent @event);
}