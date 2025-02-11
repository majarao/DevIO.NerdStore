using DevIO.NerdStore.BFF.Compras.Models;

namespace DevIO.NerdStore.BFF.Compras.Services;

public interface IPedidoService
{
    Task<VoucherDTO?> ObterVoucherPorCodigo(string codigo);
}
