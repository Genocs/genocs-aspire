using Genocs.BlazorAspire.Shared.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Genocs.BlazorAspire.Infrastructure.Auth;

public class MustHavePermissionAttribute : AuthorizeAttribute
{
    public MustHavePermissionAttribute(string action, string resource)
        => Policy = GNXPermission.NameFor(action, resource);
}