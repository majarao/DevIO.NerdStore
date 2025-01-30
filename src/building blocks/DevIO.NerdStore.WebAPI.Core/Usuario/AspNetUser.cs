using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DevIO.NerdStore.WebAPI.Core.Usuario;

public class AspNetUser(IHttpContextAccessor accessor) : IAspNetUser
{
    private IHttpContextAccessor Accessor { get; } = accessor;

    public string? Name => Accessor.HttpContext?.User?.Identity?.Name;

    public Guid? ObterUserId()
    {
        if (EstaAutenticado())
        {
            if (Guid.TryParse(Accessor.HttpContext?.User.GetUserId(), out Guid result))
                return result;
        }

        return null;
    }

    public string? ObterUserEmail() =>
        EstaAutenticado() ? Accessor.HttpContext?.User.GetUserEmail() : "";

    public string? ObterUserToken() =>
        EstaAutenticado() ? Accessor.HttpContext?.User.GetUserToken() : "";

    public bool EstaAutenticado() =>
        Accessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;

    public bool PossuiRole(string role) =>
        Accessor.HttpContext?.User.IsInRole(role) ?? false;

    public IEnumerable<Claim>? ObterClaims() =>
        Accessor.HttpContext?.User.Claims;

    public HttpContext? ObterHttpContext() =>
        Accessor.HttpContext;
}
