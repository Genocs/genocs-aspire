using Genocs.Core.CQRS.Commands;

namespace GenocsAspire.Identities.Application.Commands;

public class RevokeAccessToken : ICommand
{
    public string AccessToken { get; }

    public RevokeAccessToken(string accessToken)
    {
        AccessToken = accessToken;
    }
}