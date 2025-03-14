﻿using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.Identity.API.Models;
using DevIO.NerdStore.MessageBus;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using DevIO.NerdStore.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DevIO.NerdStore.Identity.API.Controllers;

[Route("api/identity")]
public class AuthController(
    UserManager<IdentityUser> userManager,
    SignInManager<IdentityUser> signInManager,
    IOptions<AppSettings> appSettings,
    IMessageBus bus) : MainController
{
    private UserManager<IdentityUser> UserManager { get; } = userManager;
    private SignInManager<IdentityUser> SignInManager { get; } = signInManager;
    private AppSettings AppSettings { get; } = appSettings.Value;
    private IMessageBus Bus { get; } = bus;

    [HttpPost("registrar")]
    public async Task<ActionResult> Registrar(UsuarioRegistro usuarioRegistro)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        IdentityUser user = new()
        {
            UserName = usuarioRegistro.Email,
            Email = usuarioRegistro.Email,
            EmailConfirmed = true
        };

        IdentityResult result = await UserManager.CreateAsync(user, usuarioRegistro.Senha);

        if (result.Succeeded)
        {
            ResponseMessage clienteRegistrado = await RegistrarCliente(usuarioRegistro);

            if (!clienteRegistrado.ValidationResult.IsValid)
            {
                await UserManager.DeleteAsync(user);
                return CustomResponse(clienteRegistrado.ValidationResult);
            }

            return CustomResponse(await GerarJwt(usuarioRegistro.Email));
        }

        foreach (IdentityError error in result.Errors)
            AdicionarErroProcessamento(error.Description);

        return CustomResponse();
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(UsuarioLogin usuarioLogin)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        Microsoft.AspNetCore.Identity.SignInResult result = await SignInManager.PasswordSignInAsync(usuarioLogin.Email, usuarioLogin.Senha, false, true);

        if (result.Succeeded)
            return CustomResponse(await GerarJwt(usuarioLogin.Email));

        if (result.IsLockedOut)
        {
            AdicionarErroProcessamento("Usuário temporiamente bloqueado por tentativa inválidas");
            return CustomResponse();
        }

        AdicionarErroProcessamento("Usuário ou senha incorretos");
        return CustomResponse();
    }

    private async Task<UsuarioRespostaLogin> GerarJwt(string email)
    {
        IdentityUser user = await UserManager.FindByEmailAsync(email) ?? new();
        IList<Claim> claims = await UserManager.GetClaimsAsync(user);

        ClaimsIdentity claimsIdentity = await ObterClaimsUsuario(claims, user);
        string encodedToken = CodificarToken(claimsIdentity);

        UsuarioRespostaLogin response = ObterRespostaToken(encodedToken, user, claims);

        return response;
    }

    private async Task<ClaimsIdentity> ObterClaimsUsuario(ICollection<Claim> claims, IdentityUser user)
    {
        IList<string> roles = await UserManager.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email!));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

        foreach (string role in roles)
            claims.Add(new Claim("role", role));

        ClaimsIdentity identityClaims = new();
        identityClaims.AddClaims(claims);

        return identityClaims;
    }

    private string CodificarToken(ClaimsIdentity claimsIdentity)
    {
        JwtSecurityTokenHandler tokenHandler = new();
        byte[] key = Encoding.ASCII.GetBytes(AppSettings.Secret);

        SecurityToken token = tokenHandler.CreateToken(
            new SecurityTokenDescriptor
            {
                Issuer = AppSettings.Emissor,
                Audience = AppSettings.ValidoEm,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddHours(AppSettings.ExpiracaoHoras),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

        string encodedToken = tokenHandler.WriteToken(token);

        return encodedToken;
    }

    private UsuarioRespostaLogin ObterRespostaToken(string encodedToken, IdentityUser user, IEnumerable<Claim> claims)
    {
        UsuarioRespostaLogin response = new()
        {
            AccessToken = encodedToken,
            ExpiresIn = TimeSpan.FromHours(AppSettings.ExpiracaoHoras).TotalSeconds,
            UsuarioToken = new UsuarioToken
            {
                Id = user.Id,
                Email = user.Email ?? string.Empty,
                Claims = claims.Select(c => new UsuarioClaim { Type = c.Type, Value = c.Value })
            }
        };

        return response;
    }

    private async Task<ResponseMessage> RegistrarCliente(UsuarioRegistro usuarioRegistro)
    {
        IdentityUser? usuario = await UserManager.FindByEmailAsync(usuarioRegistro.Email);

        ArgumentNullException.ThrowIfNull(usuario);

        UsuarioRegistradoIntegrationEvent usuarioRegistrado = new(Guid.Parse(usuario.Id), usuarioRegistro.Nome, usuarioRegistro.Email, usuarioRegistro.Cpf);

        try
        {
            return await Bus.RequestAsync<UsuarioRegistradoIntegrationEvent, ResponseMessage>(usuarioRegistrado);
        }
        catch
        {
            await UserManager.DeleteAsync(usuario);
            throw;
        }
    }

    private static long ToUnixEpochDate(DateTime date)
        => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
}
