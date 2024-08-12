using GenocsAspire.Multitenancy.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace GenocsAspire.Multitenancy.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource)
        => Policy = GNXPermission.NameFor(action, resource);
}