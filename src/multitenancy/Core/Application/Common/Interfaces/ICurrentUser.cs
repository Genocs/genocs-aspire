using System.Security.Claims;

namespace GenocsAspire.Multitenancy.Application.Common.Interfaces;

public interface ICurrentUser
{
    string? Name { get; }

    DefaultIdType GetUserId();

    string? GetUserEmail();

    string? GetTenant();

    bool IsAuthenticated();

    bool IsInRole(string role);

    IEnumerable<Claim>? GetUserClaims();
}