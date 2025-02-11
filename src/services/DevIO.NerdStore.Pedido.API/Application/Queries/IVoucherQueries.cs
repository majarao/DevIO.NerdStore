using DevIO.NerdStore.Pedido.API.Application.DTO;

namespace DevIO.NerdStore.Pedido.API.Application.Queries;

public interface IVoucherQueries
{
    Task<VoucherDTO?> ObterVoucherPorCodigo(string codigo);
}
