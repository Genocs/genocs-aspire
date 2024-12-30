using Genocs.MultitenancyAspire.Shared.Multitenancy;

namespace Genocs.MultitenancyAspire.Infrastructure.OpenApi;

public class TenantIdHeaderAttribute : SwaggerHeaderAttribute
{
    public TenantIdHeaderAttribute()
        : base(
                MultitenancyConstants.TenantIdName,
                "Input your tenant Id to access this API",
                string.Empty,
                true)
    {
    }
}
