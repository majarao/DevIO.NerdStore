using DevIO.NerdStore.Core.DomainObjects;
using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.Pagamentos.API.Facade;
using DevIO.NerdStore.Pagamentos.API.Models;
using FluentValidation.Results;

namespace DevIO.NerdStore.Pagamentos.API.Services;

public class PagamentoService(IPagamentoFacade pagamentoFacade, IPagamentoRepository pagamentoRepository) : IPagamentoService
{
    private IPagamentoFacade Facade { get; } = pagamentoFacade;
    private IPagamentoRepository Repository { get; } = pagamentoRepository;

    public async Task<ResponseMessage> AutorizarPagamento(Pagamento pagamento)
    {
        Transacao transacao = await Facade.AutorizarPagamento(pagamento);
        ValidationResult validationResult = new();

        if (transacao.Status != StatusTransacao.Autorizado)
        {
            validationResult.Errors.Add(new("Pagamento", "Pagamento recusado, entre em contato com a sua operadora de cartão"));

            return new(validationResult);
        }

        pagamento.AdicionarTransacao(transacao);

        Repository.AdicionarPagamento(pagamento);

        if (!await Repository.UnitOfWork.Commit())
        {
            validationResult.Errors.Add(new("Pagamento", "Houve um erro ao realizar o pagamento."));

            return new(validationResult);
        }

        return new(validationResult);
    }

    public async Task<ResponseMessage> CapturarPagamento(Guid pedidoId)
    {
        IEnumerable<Transacao> transacoes = await Repository.ObterTransacaoesPorPedidoId(pedidoId);

        Transacao? transacaoAutorizada = transacoes?.FirstOrDefault(t => t.Status == StatusTransacao.Autorizado);

        ValidationResult validationResult = new();

        if (transacaoAutorizada is null)
            throw new DomainException($"Transação não encontrada para o pedido {pedidoId}");

        Transacao transacao = await Facade.CapturarPagamento(transacaoAutorizada);

        if (transacao.Status != StatusTransacao.Pago)
        {
            validationResult.Errors.Add(new("Pagamento", $"Não foi possível capturar o pagamento do pedido {pedidoId}"));

            return new(validationResult);
        }

        transacao.PagamentoId = transacaoAutorizada.PagamentoId;
        Repository.AdicionarTransacao(transacao);

        if (!await Repository.UnitOfWork.Commit())
        {
            validationResult.Errors.Add(new("Pagamento", $"Não foi possível persistir a captura do pagamento do pedido {pedidoId}"));

            return new(validationResult);
        }

        return new(validationResult);
    }

    public async Task<ResponseMessage> CancelarPagamento(Guid pedidoId)
    {
        IEnumerable<Transacao> transacoes = await Repository.ObterTransacaoesPorPedidoId(pedidoId);

        Transacao? transacaoAutorizada = transacoes?.FirstOrDefault(t => t.Status == StatusTransacao.Autorizado);

        ValidationResult validationResult = new();

        if (transacaoAutorizada is null)
            throw new DomainException($"Transação não encontrada para o pedido {pedidoId}");

        Transacao transacao = await Facade.CancelarAutorizacao(transacaoAutorizada);

        if (transacao.Status != StatusTransacao.Cancelado)
        {
            validationResult.Errors.Add(new("Pagamento", $"Não foi possível cancelar o pagamento do pedido {pedidoId}"));

            return new(validationResult);
        }

        transacao.PagamentoId = transacaoAutorizada.PagamentoId;
        Repository.AdicionarTransacao(transacao);

        if (!await Repository.UnitOfWork.Commit())
        {
            validationResult.Errors.Add(new("Pagamento", $"Não foi possível persistir o cancelamento do pagamento do pedido {pedidoId}"));

            return new(validationResult);
        }

        return new(validationResult);
    }
}
