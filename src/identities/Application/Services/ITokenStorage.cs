using GenocsAspire.Identities.Application.DTO;

namespace GenocsAspire.Identities.Application.Services;

public interface ITokenStorage
{
    void Set(Guid commandId, AuthDto token);
    AuthDto Get(Guid commandId);
}