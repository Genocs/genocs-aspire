using Genocs.Core.CQRS.Events;

namespace GenocsAspire.Identities.Application.Services;

public interface IMessageBroker
{
    Task PublishAsync(params IEvent[] events);
}