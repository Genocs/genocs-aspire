using GenocsAspire.Identities.Application.Domain.Entities;

namespace GenocsAspire.Identities.Application.Domain.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken> GetAsync(string token);
    Task AddAsync(RefreshToken refreshToken);
    Task UpdateAsync(RefreshToken refreshToken);
}