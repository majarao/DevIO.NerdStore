using DevIO.NerdStore.Pedidos.API.Application.DTO;

namespace DevIO.NerdStore.Pedidos.API.Application.Queries;

public interface IPedidoQueries
{
    Task<PedidoDTO> ObterUltimoPedido(Guid clienteId);

    Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId(Guid clienteId);

    Task<PedidoDTO?> ObterPedidosAutorizados();
}
