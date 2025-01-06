using System.Security.Claims;

namespace DevIO.NerdStore.WebApp.MVC.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string? GetUserId(this ClaimsPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentException(null, nameof(principal));

        Claim? claim = principal.FindFirst("sub");
        return claim?.Value;
    }

    public static string? GetUserEmail(this ClaimsPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentException(null, nameof(principal));

        Claim? claim = principal.FindFirst("email");
        return claim?.Value;
    }

    public static string? GetUserToken(this ClaimsPrincipal principal)
    {
        if (principal is null)
            throw new ArgumentException(null, nameof(principal));

        Claim? claim = principal.FindFirst("JWT");
        return claim?.Value;
    }
}
