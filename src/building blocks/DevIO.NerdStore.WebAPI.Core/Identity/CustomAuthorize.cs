using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace DevIO.NerdStore.WebAPI.Core.Identity;

public class CustomAuthorization
{
    public static bool ValidarClaimsUsuario(HttpContext? context, string claimName, string claimValue) =>
        (context?.User?.Identity?.IsAuthenticated ?? false) && context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
}

public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitoClaimFilter))
    {
        Arguments = [claimName, claimValue];
    }
}

public class RequisitoClaimFilter(Claim claim) : IAuthorizationFilter
{
    private Claim Claim { get; } = claim;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext?.User?.Identity?.IsAuthenticated ?? false)
        {
            context.Result = new StatusCodeResult(401);
            return;
        }

        if (!CustomAuthorization.ValidarClaimsUsuario(context.HttpContext, Claim.Type, Claim.Value))
            context.Result = new StatusCodeResult(403);
    }
}
