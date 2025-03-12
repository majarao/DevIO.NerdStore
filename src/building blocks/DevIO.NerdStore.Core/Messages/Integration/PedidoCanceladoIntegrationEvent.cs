namespace DevIO.NerdStore.Core.Messages.Integration;

public class PedidoCanceladoIntegrationEvent(Guid clienteId, Guid pedidoId) : IntegrationEvent
{
    public Guid ClienteId { get; private set; } = clienteId;
    public Guid PedidoId { get; private set; } = pedidoId;
}