using GenocsAspire.Multitenancy.Application.Common.Interfaces;
using GenocsAspire.Multitenancy.Shared.Authorization;
using System.Security.Claims;

namespace GenocsAspire.Multitenancy.Infrastructure.Auth;

public class CurrentUser : ICurrentUser, ICurrentUserInitializer
{
    private ClaimsPrincipal? _user;

    public string? Name
        => _user?.Identity?.Name;

    private DefaultIdType _userId = DefaultIdType.Empty;

    public DefaultIdType GetUserId()
        => IsAuthenticated()
            ? DefaultIdType.Parse(_user?.GetUserId() ?? DefaultIdType.Empty.ToString())
            : _userId;

    public string? GetUserEmail()
        => IsAuthenticated()
            ? _user!.GetEmail()
            : string.Empty;

    public bool IsAuthenticated()
        => _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role)
        => _user?.IsInRole(role) is true;

    public IEnumerable<Claim>? GetUserClaims()
        => _user?.Claims;

    public string? GetTenant()
        => IsAuthenticated() ? _user?.GetTenant() : string.Empty;

    public void SetCurrentUser(ClaimsPrincipal user)
    {
        if (_user != null)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        _user = user;
    }

    public void SetCurrentUserId(string userId)
    {
        if (_userId != DefaultIdType.Empty)
        {
            throw new Exception("Method reserved for in-scope initialization");
        }

        if (!string.IsNullOrEmpty(userId))
        {
            _userId = DefaultIdType.Parse(userId);
        }
    }
}