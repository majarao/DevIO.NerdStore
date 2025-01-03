using DevIO.NerdStore.Identity.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.Identity.API.Controllers;

[Route("api/identity")]
public class AuthController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager) : Controller
{
    private UserManager<IdentityUser> UserManager { get; } = userManager;
    private SignInManager<IdentityUser> SignInManager { get; } = signInManager;

    [HttpPost("registrar")]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        IdentityUser user = new()
        {
            UserName = usuarioRegistro.Email,
            Email = usuarioRegistro.Email,
            EmailConfirmed = true
        };

        IdentityResult result = await UserManager.CreateAsync(user, usuarioRegistro.Senha);

        if (result.Succeeded)
            return Ok();

        return BadRequest();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        Microsoft.AspNetCore.Identity.SignInResult result = await SignInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

        if (result.Succeeded)
            return Ok();

        return BadRequest();
    }
}
