using DevIO.NerdStore.Core.Messages;

namespace DevIO.NerdStore.Pedidos.API.Application.Events;

public class PedidoRealizadoEvent(Guid pedidoId, Guid clienteId) : Event
{
    public Guid PedidoId { get; private set; } = pedidoId;
    public Guid ClienteId { get; private set; } = clienteId;
}
