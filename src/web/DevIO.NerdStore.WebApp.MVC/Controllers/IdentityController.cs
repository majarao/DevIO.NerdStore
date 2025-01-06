using DevIO.NerdStore.WebApp.MVC.Models;
using DevIO.NerdStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DevIO.NerdStore.WebApp.MVC.Controllers;

public class IdentityController(IAutenticacaoService autenticacaoService) : MainController
{
    private IAutenticacaoService AutenticacaoService { get; } = autenticacaoService;

    [HttpGet]
    [Route("nova-conta")]
    public IActionResult Registro() => View();

    [HttpPost]
    [Route("nova-conta")]
    public async Task<IActionResult> Registro(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid)
            return View(usuarioRegistro);

        UsuarioRespostaLogin? resposta = await AutenticacaoService.Registro(usuarioRegistro);

        if (ResponsePossuiErros(resposta?.ResponseResult))
            return View(usuarioRegistro);

        await RealizarLogin(resposta!);

        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [Route("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        return View();
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(UsuarioLogin usuarioLogin, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)
            return View(usuarioLogin);

        UsuarioRespostaLogin? resposta = await AutenticacaoService.Login(usuarioLogin);

        if (ResponsePossuiErros(resposta?.ResponseResult))
            return View(usuarioLogin);

        await RealizarLogin(resposta!);

        if (string.IsNullOrEmpty(returnUrl))
            return RedirectToAction("Index", "Home");

        return LocalRedirect(returnUrl);
    }

    [HttpGet]
    [Route("sair")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }

    private async Task RealizarLogin(UsuarioRespostaLogin resposta)
    {
        JwtSecurityToken token = ObterTokenFormatado(resposta.AccessToken);

        List<Claim> claims = [];
        claims.Add(new Claim("JWT", resposta.AccessToken));
        claims.AddRange(token.Claims);

        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        AuthenticationProperties authProperties = new()
        {
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            IsPersistent = true
        };

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties);
    }

    private static JwtSecurityToken ObterTokenFormatado(string jwtToken) => (JwtSecurityToken)new JwtSecurityTokenHandler().ReadToken(jwtToken);
}
