using DevIO.NerdStore.Core.Mediator;
using DevIO.NerdStore.Pedidos.API.Application.Commands;
using DevIO.NerdStore.Pedidos.API.Application.DTO;
using DevIO.NerdStore.Pedidos.API.Application.Queries;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using DevIO.NerdStore.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.Pedidos.API.Controllers;

[Authorize]
public class PedidoController(IMediatorHandler mediator, IAspNetUser user, IPedidoQueries queries) : MainController
{
    private IMediatorHandler Mediator { get; } = mediator;
    private new IAspNetUser User { get; } = user;
    private IPedidoQueries Queries { get; } = queries;

    [HttpPost("pedido")]
    public async Task<IActionResult> AdicionarPedido(AdicionarPedidoCommand pedido)
    {
        pedido.ClienteId = User.ObterUserId() ?? Guid.Empty;

        return CustomResponse(await Mediator.EnviarComando(pedido));
    }

    [HttpGet("pedido/ultimo")]
    public async Task<IActionResult> UltimoPedido()
    {
        PedidoDTO pedido = await Queries.ObterUltimoPedido(User.ObterUserId() ?? Guid.Empty);

        return pedido is null ? NotFound() : CustomResponse(pedido);
    }

    [HttpGet("pedido/lista-cliente")]
    public async Task<IActionResult> ListaPorCliente()
    {
        IEnumerable<PedidoDTO> pedidos = await Queries.ObterListaPorClienteId(User.ObterUserId() ?? Guid.Empty);

        return pedidos is null ? NotFound() : CustomResponse(pedidos);
    }
}
