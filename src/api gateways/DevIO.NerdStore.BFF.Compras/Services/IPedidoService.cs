using DevIO.NerdStore.BFF.Compras.Models;
using DevIO.NerdStore.Core.Communication;

namespace DevIO.NerdStore.BFF.Compras.Services;

public interface IPedidoService
{
    Task<VoucherDTO?> ObterVoucherPorCodigo(string codigo);

    Task<ResponseResult?> FinalizarPedido(PedidoDTO pedido);

    Task<PedidoDTO?> ObterUltimoPedido();

    Task<IEnumerable<PedidoDTO>> ObterListaPorClienteId();
}
