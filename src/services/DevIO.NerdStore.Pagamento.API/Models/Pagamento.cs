using DevIO.NerdStore.Core.DomainObjects;

namespace DevIO.NerdStore.Pagamentos.API.Models;

public class Pagamento : Entity, IAggregateRoot
{
    protected Pagamento() { }

    public Guid PedidoId { get; set; }
    public decimal Valor { get; set; }
    public CartaoCredito CartaoCredito { get; set; } = null!;
    public ICollection<Transacao> Transacoes { get; set; } = [];

    public void AdicionarTransacao(Transacao transacao) => Transacoes.Add(transacao);
}