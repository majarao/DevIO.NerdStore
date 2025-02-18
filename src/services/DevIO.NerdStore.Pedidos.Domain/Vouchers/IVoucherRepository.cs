using DevIO.NerdStore.Core.Data;

namespace DevIO.NerdStore.Pedidos.Domain.Vouchers;

public interface IVoucherRepository : IRepository<Voucher>
{
    Task<Voucher?> ObterVoucherPorCodigo(string codigo);

    void Atualizar(Voucher voucher);
}
