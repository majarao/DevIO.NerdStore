using DevIO.NerdStore.Core.Messages.Integration;
using DevIO.NerdStore.MessageBus;
using MediatR;

namespace DevIO.NerdStore.Pedidos.API.Application.Events;

public class PedidoEventHandler(IMessageBus bus) : INotificationHandler<PedidoRealizadoEvent>
{
    private IMessageBus Bus { get; } = bus;

    public async Task Handle(PedidoRealizadoEvent message, CancellationToken cancellationToken)
    {
        await Bus.PublishAsync(new PedidoRealizadoIntegrationEvent(message.ClienteId));
    }
}