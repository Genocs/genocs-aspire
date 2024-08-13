namespace GenocsAspire.Identities.Application.Domain.Exceptions;

public class InvalidRoleException : DomainException
{
    public InvalidRoleException(string role) : base($"Invalid role: {role}.")
    {
    }
}