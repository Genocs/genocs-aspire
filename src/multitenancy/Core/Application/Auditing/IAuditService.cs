using GenocsAspire.Multitenancy.Application.Common.Interfaces;

namespace GenocsAspire.Multitenancy.Application.Auditing;

public interface IAuditService : ITransientService
{
    Task<List<AuditDto>> GetUserTrailsAsync(DefaultIdType userId);
}