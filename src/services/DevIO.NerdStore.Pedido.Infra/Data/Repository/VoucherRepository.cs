using DevIO.NerdStore.Core.Data;
using DevIO.NerdStore.Pedido.Domain.Vouchers;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Pedido.Infra.Data.Repository;

public class VoucherRepository(PedidosContext context) : IVoucherRepository
{
    private PedidosContext Context { get; } = context;

    public IUnitOfWork UnitOfWork => Context;

    public async Task<Voucher?> ObterVoucherPorCodigo(string codigo) =>
        await Context.Vouchers.FirstOrDefaultAsync(p => p.Codigo == codigo);

    public void Dispose() => Context.Dispose();
}
