using Genocs.Core.CQRS.Commands;

namespace GenocsAspire.Identities.Application.Commands;

public class UnlockUser : ICommand
{
    public Guid UserId { get; }

    public UnlockUser(Guid userId)
    {
        UserId = userId;
    }
}