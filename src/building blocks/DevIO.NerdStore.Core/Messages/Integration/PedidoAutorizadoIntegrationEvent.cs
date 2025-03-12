namespace DevIO.NerdStore.Core.Messages.Integration;

public class PedidoAutorizadoIntegrationEvent(Guid clienteId, Guid pedidoId, IDictionary<Guid, int> itens) : IntegrationEvent
{
    public Guid ClienteId { get; private set; } = clienteId;
    public Guid PedidoId { get; private set; } = pedidoId;
    public IDictionary<Guid, int> Itens { get; private set; } = itens;
}