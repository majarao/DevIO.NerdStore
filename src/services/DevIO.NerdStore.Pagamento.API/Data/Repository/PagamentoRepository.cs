using DevIO.NerdStore.Core.Data;
using DevIO.NerdStore.Pagamentos.API.Models;

namespace DevIO.NerdStore.Pagamentos.API.Data.Repository;

public class PagamentoRepository(PagamentosContext context) : IPagamentoRepository
{
    private PagamentosContext Context { get; } = context;
    public IUnitOfWork UnitOfWork => Context;

    public void AdicionarPagamento(Pagamento pagamento) => Context.Pagamentos.Add(pagamento);

    public void Dispose() => GC.SuppressFinalize(this);
}
