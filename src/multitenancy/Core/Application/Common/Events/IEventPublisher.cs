using Genocs.MultitenancyAspire.Shared.Events;

namespace Genocs.MultitenancyAspire.Application.Common.Events;

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