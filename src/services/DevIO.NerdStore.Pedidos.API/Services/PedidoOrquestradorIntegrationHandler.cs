namespace DevIO.NerdStore.Pedidos.API.Services;

public class PedidoOrquestradorIntegrationHandler : IHostedService, IDisposable
{
    private Timer? Timer { get; set; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Timer = new(ProcessarPedidos, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));

        return Task.CompletedTask;
    }

    private void ProcessarPedidos(object? state)
    {

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose() => Timer?.Dispose();
}
