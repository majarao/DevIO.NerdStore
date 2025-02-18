using DevIO.NerdStore.Core.Data;
using DevIO.NerdStore.Pedidos.Domain.Vouchers;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Pedidos.Infra.Data.Repository;

public class VoucherRepository(PedidosContext context) : IVoucherRepository
{
    private PedidosContext Context { get; } = context;

    public IUnitOfWork UnitOfWork => Context;

    public async Task<Voucher?> ObterVoucherPorCodigo(string codigo) =>
        await Context.Vouchers.FirstOrDefaultAsync(p => p.Codigo == codigo);

    public void Atualizar(Voucher voucher) => Context.Vouchers.Update(voucher);

    public void Dispose() => GC.SuppressFinalize(this);
}
