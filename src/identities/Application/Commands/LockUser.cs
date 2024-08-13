using Genocs.Core.CQRS.Commands;

namespace GenocsAspire.Identities.Application.Commands;

public class LockUser : ICommand
{
    public Guid UserId { get; }

    public LockUser(Guid userId)
    {
        UserId = userId;
    }
}