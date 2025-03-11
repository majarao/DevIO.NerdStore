using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.MessageBus;
using DevIO.NerdStore.Pagamentos.API.Models;

namespace DevIO.NerdStore.Pagamentos.API.Services;

public class PagamentoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus) : BackgroundService
{
    private IMessageBus Bus { get; } = bus;
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetResponder();
        return Task.CompletedTask;
    }

    private void SetResponder() =>
        Bus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request => await AutorizarPagamento(request));

    private async Task<ResponseMessage> AutorizarPagamento(PedidoIniciadoIntegrationEvent message)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();

        IPagamentoService pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

        Pagamento pagamento = new()
        {
            PedidoId = message.PedidoId,
            Valor = message.Valor,
            CartaoCredito = new(message.NomeCartao, message.NumeroCartao, message.MesAnoVencimento, message.CVV)
        };

        ResponseMessage response = await pagamentoService.AutorizarPagamento(pagamento);

        return response;
    }
}
