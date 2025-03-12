using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.MessageBus;
using DevIO.NerdStore.Pedidos.API.Application.Queries;

namespace DevIO.NerdStore.Pedidos.API.Services;

public class PedidoOrquestradorIntegrationHandler(IServiceProvider serviceProvider) : IHostedService, IDisposable
{
    private Timer? Timer { get; set; }
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Timer = new(ProcessarPedidos, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

        return Task.CompletedTask;
    }

    private async void ProcessarPedidos(object? state)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();

        IPedidoQueries pedidoQueries = scope.ServiceProvider.GetRequiredService<IPedidoQueries>();

        Application.DTO.PedidoDTO? pedido = await pedidoQueries.ObterPedidosAutorizados();

        if (pedido == null)
            return;

        IMessageBus bus = scope.ServiceProvider.GetRequiredService<IMessageBus>();

        PedidoAutorizadoIntegrationEvent pedidoAutorizado = new(pedido.ClienteId, pedido.Id, pedido.PedidoItems.ToDictionary(p => p.ProdutoId, p => p.Quantidade));

        await bus.PublishAsync(pedidoAutorizado);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose() => Timer?.Dispose();
}
