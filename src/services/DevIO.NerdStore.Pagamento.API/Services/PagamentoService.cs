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
            validationResult.Errors.Add(new ValidationFailure("Pagamento",
                    "Pagamento recusado, entre em contato com a sua operadora de cartão"));

            return new(validationResult);
        }

        pagamento.AdicionarTransacao(transacao);

        Repository.AdicionarPagamento(pagamento);

        if (!await Repository.UnitOfWork.Commit())
        {
            validationResult.Errors.Add(new ValidationFailure("Pagamento",
                "Houve um erro ao realizar o pagamento."));

            return new(validationResult);
        }

        return new(validationResult);
    }
}
