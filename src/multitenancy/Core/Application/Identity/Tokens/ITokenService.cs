using GenocsAspire.Multitenancy.Application.Common.Interfaces;

namespace GenocsAspire.Multitenancy.Application.Identity.Tokens;

public interface ITokenService : ITransientService
{
    Task<TokenResponse> GetTokenAsync(TokenRequest request, string ipAddress, CancellationToken cancellationToken);

    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request, string ipAddress);
}