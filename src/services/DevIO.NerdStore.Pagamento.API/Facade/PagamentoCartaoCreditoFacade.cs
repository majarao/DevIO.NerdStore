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

    public async Task<Transacao> CapturarPagamento(Transacao transacao)
    {
        NerdsPagService nerdsPagSvc = new(PagamentoConfig.DefaultApiKey, PagamentoConfig.DefaultEncryptionKey);

        Transaction transaction = ParaTransaction(transacao, nerdsPagSvc);

        return ParaTransacao(await transaction.CaptureCardTransaction());
    }

    public async Task<Transacao> CancelarAutorizacao(Transacao transacao)
    {
        NerdsPagService nerdsPagSvc = new(PagamentoConfig.DefaultApiKey, PagamentoConfig.DefaultEncryptionKey);

        Transaction transaction = ParaTransaction(transacao, nerdsPagSvc);

        return ParaTransacao(await transaction.CancelAuthorization());
    }

    private static Transacao ParaTransacao(Transaction transaction)
    {
        return new()
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

    public static Transaction ParaTransaction(Transacao transacao, NerdsPagService nerdsPagService)
    {
        return new(nerdsPagService)
        {
            Status = (TransactionStatus)transacao.Status,
            Amount = transacao.ValorTotal,
            CardBrand = transacao.BandeiraCartao,
            AuthorizationCode = transacao.CodigoAutorizacao,
            Cost = transacao.CustoTransacao,
            Nsu = transacao.NSU,
            Tid = transacao.TID
        };
    }
}
