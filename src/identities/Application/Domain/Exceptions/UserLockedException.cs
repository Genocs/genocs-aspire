namespace GenocsAspire.Identities.Application.Domain.Exceptions;

public class UserLockedException : DomainException
{
    public Guid UserId { get; }

    public UserLockedException(Guid userId) : base($"User with ID: '{userId}' is locked.")
    {
        UserId = userId;
    }
}