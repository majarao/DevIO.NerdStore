using System.Security.Claims;

namespace DevIO.NerdStore.WebApp.MVC.Extensions;

public class AspNetUser(IHttpContextAccessor accessor) : IUser
{
    private IHttpContextAccessor Accessor { get; } = accessor;

    public string Name => Accessor.HttpContext!.User!.Identity!.Name!;

    public Guid ObterUserId() =>
        EstaAutenticado() ? Guid.Parse(Accessor.HttpContext!.User!.GetUserId()!) : Guid.Empty;

    public string ObterUserEmail() =>
        EstaAutenticado() ? Accessor.HttpContext!.User!.GetUserEmail()! : "";

    public string ObterUserToken() =>
        EstaAutenticado() ? Accessor.HttpContext!.User!.GetUserToken()! : "";

    public bool EstaAutenticado() =>
        Accessor.HttpContext!.User!.Identity!.IsAuthenticated;

    public bool PossuiRole(string role) =>
        Accessor.HttpContext!.User.IsInRole(role);

    public IEnumerable<Claim> ObterClaims() =>
        Accessor.HttpContext!.User.Claims;

    public HttpContext ObterHttpContext() =>
        Accessor.HttpContext!;
}
