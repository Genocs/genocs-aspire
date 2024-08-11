using Genocs.Core.CQRS.Events;

namespace GenocsAspire.Identities.Application.Events;

public class SignedIn : IEvent
{
    public Guid UserId { get; }

    public SignedIn(Guid userId)
    {
        UserId = userId;
    }
}