using Genocs.MultitenancyAspire.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Genocs.MultitenancyAspire.Infrastructure.Auth.Permissions;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource)
        => Policy = GNXPermission.NameFor(action, resource);
}