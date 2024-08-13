using Genocs.Core.CQRS.Events;

namespace GenocsAspire.Identities.Application.Events;

public class UserLocked : IEvent
{
    public Guid UserId { get; }

    public UserLocked(Guid userId)
    {
        UserId = userId;
    }
}