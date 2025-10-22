using System.Security.Claims;

namespace EzyClassroomz.Api.Extensions;

public static class AuthenticationExtensions
{
    public static string? GetTenantId(this ClaimsPrincipal user)
    {
        var tenantIdClaim = user.Claims
            .Where(c => c.Type.Equals("tenantid", StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        return tenantIdClaim?.Value;
    }
}