using DevIO.NerdStore.Core.Data;
using DevIO.NerdStore.Pagamentos.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Pagamentos.API.Data.Repository;

public class PagamentoRepository(PagamentosContext context) : IPagamentoRepository
{
    private PagamentosContext Context { get; } = context;
    public IUnitOfWork UnitOfWork => Context;

    public void AdicionarPagamento(Pagamento pagamento) => Context.Pagamentos.Add(pagamento);

    public void AdicionarTransacao(Transacao transacao) => Context.Transacoes.Add(transacao);

    public async Task<Pagamento?> ObterPagamentoPorPedidoId(Guid pedidoId) =>
        await Context.Pagamentos.AsNoTracking().SingleOrDefaultAsync(p => p.PedidoId == pedidoId);

    public async Task<IEnumerable<Transacao>> ObterTransacaoesPorPedidoId(Guid pedidoId) =>
        await Context.Transacoes.AsNoTracking().Where(t => t.Pagamento.PedidoId == pedidoId).ToListAsync();

    public void Dispose() => GC.SuppressFinalize(this);
}
