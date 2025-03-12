using DevIO.NerdStore.Core.DomainObjects;
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

        SetSubscribers();

        return Task.CompletedTask;
    }

    private void SetResponder() =>
        Bus.RespondAsync<PedidoIniciadoIntegrationEvent, ResponseMessage>(async request => await AutorizarPagamento(request));

    private void SetSubscribers()
    {
        Bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request => await CancelarPagamento(request));

        Bus.SubscribeAsync<PedidoBaixadoEstoqueIntegrationEvent>("PedidoBaixadoEstoque", async request => await CapturarPagamento(request));
    }

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

    private async Task CancelarPagamento(PedidoCanceladoIntegrationEvent message)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();

        IPagamentoService pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

        ResponseMessage response = await pagamentoService.CancelarPagamento(message.PedidoId);

        if (!response.ValidationResult.IsValid)
            throw new DomainException($"Falha ao cancelar pagamento do pedido {message.PedidoId}");
    }

    private async Task CapturarPagamento(PedidoBaixadoEstoqueIntegrationEvent message)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();

        IPagamentoService pagamentoService = scope.ServiceProvider.GetRequiredService<IPagamentoService>();

        ResponseMessage response = await pagamentoService.CapturarPagamento(message.PedidoId);

        if (!response.ValidationResult.IsValid)
            throw new DomainException($"Falha ao capturar pagamento do pedido {message.PedidoId}");

        await Bus.PublishAsync(new PedidoPagoIntegrationEvent(message.ClienteId, message.PedidoId));
    }
}
