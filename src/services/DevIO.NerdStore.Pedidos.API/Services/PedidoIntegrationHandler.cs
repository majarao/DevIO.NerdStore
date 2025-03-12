using DevIO.NerdStore.Core.DomainObjects;
using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.MessageBus;
using DevIO.NerdStore.Pedidos.Domain.Pedidos;

namespace DevIO.NerdStore.Pedidos.API.Services;

public class PedidoIntegrationHandler(IServiceProvider serviceProvider, IMessageBus bus) : BackgroundService
{
    private IMessageBus Bus { get; } = bus;
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscribers();

        return Task.CompletedTask;
    }

    private void SetSubscribers()
    {
        Bus.SubscribeAsync<PedidoCanceladoIntegrationEvent>("PedidoCancelado", async request => await CancelarPedido(request));

        Bus.SubscribeAsync<PedidoPagoIntegrationEvent>("PedidoPago", async request => await FinalizarPedido(request));
    }

    private async Task CancelarPedido(PedidoCanceladoIntegrationEvent message)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();

        IPedidoRepository pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

        Pedido? pedido = await pedidoRepository.ObterPorId(message.PedidoId);

        if (pedido is not null)
        {
            pedido.CancelarPedido();

            pedidoRepository.Atualizar(pedido);

            if (!await pedidoRepository.UnitOfWork.Commit())
                throw new DomainException($"Problemas ao cancelar o pedido {message.PedidoId}");
        }
    }

    private async Task FinalizarPedido(PedidoPagoIntegrationEvent message)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();

        IPedidoRepository pedidoRepository = scope.ServiceProvider.GetRequiredService<IPedidoRepository>();

        Pedido? pedido = await pedidoRepository.ObterPorId(message.PedidoId);

        if (pedido is not null)
        {
            pedido.FinalizarPedido();

            pedidoRepository.Atualizar(pedido);

            if (!await pedidoRepository.UnitOfWork.Commit())
                throw new DomainException($"Problemas ao finalizar o pedido {message.PedidoId}");
        }
    }
}
