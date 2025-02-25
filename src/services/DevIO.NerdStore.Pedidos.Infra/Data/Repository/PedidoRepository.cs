using DevIO.NerdStore.Core.Data;
using DevIO.NerdStore.Pedidos.Domain.Pedidos;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace DevIO.NerdStore.Pedidos.Infra.Data.Repository;

public class PedidoRepository(PedidosContext context) : IPedidoRepository
{
    private PedidosContext Context { get; } = context;

    public IUnitOfWork UnitOfWork => Context;

    public async Task<Pedido?> ObterPorId(Guid id) => await Context.Pedidos.SingleOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Pedido>> ObterListaPorClienteId(Guid clienteId) =>
        await Context.Pedidos
            .Include(p => p.PedidoItems)
            .AsNoTracking()
            .Where(p => p.ClienteId == clienteId)
            .ToListAsync();

    public void Adicionar(Pedido pedido) => Context.Pedidos.Add(pedido);

    public void Atualizar(Pedido pedido) => Context.Pedidos.Update(pedido);

    public async Task<PedidoItem?> ObterItemPorId(Guid id) => await Context.PedidoItems.SingleOrDefaultAsync(i => i.Id == id);

    public async Task<PedidoItem?> ObterItemPorPedido(Guid pedidoId, Guid produtoId) =>
        await Context.PedidoItems.FirstOrDefaultAsync(p => p.ProdutoId == produtoId && p.PedidoId == pedidoId);

    public DbConnection ObterConexao() => Context.Database.GetDbConnection();

    public void Dispose() => GC.SuppressFinalize(this);
}
