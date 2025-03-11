using DevIO.NerdStore.Pagamentos.API.Models;
using DevIO.NerdStore.Pagamentos.NerdsPag;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.Pagamentos.API.Facade;

public class PagamentoCartaoCreditoFacade(IOptions<PagamentoConfig> pagamentoConfig) : IPagamentoFacade
{
    private PagamentoConfig PagamentoConfig { get; } = pagamentoConfig.Value;

    public async Task<Transacao> AutorizarPagamento(Pagamento pagamento)
    {
        NerdsPagService nerdsPagSvc = new(PagamentoConfig.DefaultApiKey, PagamentoConfig.DefaultEncryptionKey);

        CardHash cardHashGen = new(nerdsPagSvc)
        {
            CardNumber = pagamento.CartaoCredito.NumeroCartao,
            CardHolderName = pagamento.CartaoCredito.NomeCartao,
            CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
            CardCvv = pagamento.CartaoCredito.CVV
        };

        string cardHash = cardHashGen.Generate();

        Transaction transacao = new(nerdsPagSvc)
        {
            CardHash = cardHash,
            CardNumber = pagamento.CartaoCredito.NumeroCartao,
            CardHolderName = pagamento.CartaoCredito.NomeCartao,
            CardExpirationDate = pagamento.CartaoCredito.MesAnoVencimento,
            CardCvv = pagamento.CartaoCredito.CVV,
            PaymentMethod = PaymentMethod.CreditCard,
            Amount = pagamento.Valor
        };

        return ParaTransacao(await transacao.AuthorizeCardTransaction());
    }

    private static Transacao ParaTransacao(Transaction transaction)
    {
        return new Transacao
        {
            Id = Guid.NewGuid(),
            Status = (StatusTransacao)transaction.Status,
            ValorTotal = transaction.Amount,
            BandeiraCartao = transaction.CardBrand!,
            CodigoAutorizacao = transaction.AuthorizationCode!,
            CustoTransacao = transaction.Cost,
            DataTransacao = transaction.TransactionDate,
            NSU = transaction.Nsu!,
            TID = transaction.Tid!
        };
    }
}
