using DevIO.NerdStore.Pedidos.API.Application.DTO;

namespace DevIO.NerdStore.Pedidos.API.Application.Queries;

public interface IVoucherQueries
{
    Task<VoucherDTO?> ObterVoucherPorCodigo(string codigo);
}
