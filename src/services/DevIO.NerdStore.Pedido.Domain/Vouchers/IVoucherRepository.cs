using DevIO.NerdStore.Core.Data;

namespace DevIO.NerdStore.Pedido.Domain.Vouchers;

public interface IVoucherRepository : IRepository<Voucher>
{
    Task<Voucher?> ObterVoucherPorCodigo(string codigo);
}
