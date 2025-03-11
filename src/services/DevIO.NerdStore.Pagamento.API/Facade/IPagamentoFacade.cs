using DevIO.NerdStore.Pagamentos.API.Models;

namespace DevIO.NerdStore.Pagamentos.API.Facade;

public interface IPagamentoFacade
{
    Task<Transacao> AutorizarPagamento(Pagamento pagamento);
}
