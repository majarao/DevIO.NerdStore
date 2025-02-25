namespace DevIO.NerdStore.Core.Messages.Integration;

public class PedidoRealizadoIntegrationEvent(Guid clienteId) : IntegrationEvent
{
    public Guid ClienteId { get; private set; } = clienteId;
}