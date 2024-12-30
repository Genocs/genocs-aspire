using Finbuckle.MultiTenant.Abstractions;
using Genocs.MultitenancyAspire.Application.Common.Caching;
using Genocs.MultitenancyAspire.Infrastructure.Multitenancy;

namespace Genocs.MultitenancyAspire.Infrastructure.Caching;

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