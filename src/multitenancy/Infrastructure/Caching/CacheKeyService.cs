using Finbuckle.MultiTenant.Abstractions;
using GenocsAspire.Multitenancy.Application.Common.Caching;
using GenocsAspire.Multitenancy.Infrastructure.Multitenancy;

namespace GenocsAspire.Multitenancy.Infrastructure.Caching;

public class CacheKeyService : ICacheKeyService
{
    private readonly ITenantInfo? _currentTenant;

    public CacheKeyService(IMultiTenantContextAccessor<GNXTenantInfo> multiTenantContextAccessor)
        => _currentTenant = multiTenantContextAccessor?.MultiTenantContext?.TenantInfo;

    public string GetCacheKey(string name, object id, bool includeTenantId = true)
    {
        string tenantId = includeTenantId
            ? _currentTenant?.Id ?? throw new InvalidOperationException("GetCacheKey: includeTenantId set to true and no ITenantInfo available.")
            : "GLOBAL";
        return $"{tenantId}-{name}-{id}";
    }
}