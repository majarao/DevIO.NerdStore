using DevIO.NerdStore.Core.Data;

namespace DevIO.NerdStore.Pagamentos.API.Models;

public interface IPagamentoRepository : IRepository<Pagamento>
{
    void AdicionarPagamento(Pagamento pagamento);
}