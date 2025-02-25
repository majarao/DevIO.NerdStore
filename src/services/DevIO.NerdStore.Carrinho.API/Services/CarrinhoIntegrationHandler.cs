using DevIO.NerdStore.Carrinho.API.Data;
using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.MessageBus;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Carrinho.API.Services;

public class CarrinhoIntegrationHandler(IMessageBus bus, IServiceProvider serviceProvider) : BackgroundService
{
    private IMessageBus Bus { get; } = bus;
    private IServiceProvider ServiceProvider { get; } = serviceProvider;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        SetSubscribers();

        return Task.CompletedTask;
    }

    private void SetSubscribers() =>
        Bus.SubscribeAsync<PedidoRealizadoIntegrationEvent>("PedidoRealizado", ApagarCarrinho);

    private async Task ApagarCarrinho(PedidoRealizadoIntegrationEvent message)
    {
        using IServiceScope scope = ServiceProvider.CreateScope();

        CarrinhoContext context = scope.ServiceProvider.GetRequiredService<CarrinhoContext>();

        Model.CarrinhoCliente? carrinho = await context.CarrinhoCliente.FirstOrDefaultAsync(c => c.ClienteId == message.ClienteId);

        if (carrinho is not null)
        {
            context.CarrinhoCliente.Remove(carrinho);

            await context.SaveChangesAsync();
        }
    }
}
