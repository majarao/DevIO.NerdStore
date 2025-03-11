using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.Pagamentos.API.Models;

namespace DevIO.NerdStore.Pagamentos.API.Services;

public interface IPagamentoService
{
    Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento);
}
