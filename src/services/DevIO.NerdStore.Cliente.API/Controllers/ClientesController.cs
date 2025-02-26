using DevIO.NerdStore.Clientes.API.Application.Commands;
using DevIO.NerdStore.Clientes.API.Models;
using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using DevIO.NerdStore.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.Clientes.API.Controllers;

public class ClientesController(IClienteRepository clienteRepository, IMediatorHandler mediator, IAspNetUser user) : MainController
{
    private IClienteRepository Repository { get; } = clienteRepository;
    private IMediatorHandler Mediator { get; } = mediator;
    private new IAspNetUser User { get; } = user;

    [HttpGet("cliente/endereco")]
    public async Task<IActionResult> ObterEndereco()
    {
        Guid? userId = User.ObterUserId();

        if (userId is null)
            return BadRequest();

        Endereco? endereco = await Repository.ObterEnderecoPorId((Guid)userId);

        return endereco is null ? NotFound() : CustomResponse(endereco);
    }

    [HttpPost("cliente/endereco")]
    public async Task<IActionResult> AdicionarEndereco(AdicionarEnderecoCommand endereco)
    {
        Guid? userId = User.ObterUserId();

        if (userId is null)
            return BadRequest();

        endereco.ClienteId = (Guid)userId;

        return CustomResponse(await Mediator.EnviarComando(endereco));
    }
}
