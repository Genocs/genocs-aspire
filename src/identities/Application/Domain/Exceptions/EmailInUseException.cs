namespace GenocsAspire.Identities.Application.Domain.Exceptions;

public class EmailInUseException : DomainException
{
    public string Email { get; }

    public EmailInUseException(string email) : base($"Email {email} is already in use.")
    {
        Email = email;
    }
}