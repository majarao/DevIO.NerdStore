using DevIO.NerdStore.Core.DomainObjects;

namespace DevIO.NerdStore.Pagamentos.API.Models;

public class Transacao : Entity
{
    public string CodigoAutorizacao { get; set; } = string.Empty;
    public string BandeiraCartao { get; set; } = string.Empty;
    public DateTime? DataTransacao { get; set; }
    public decimal ValorTotal { get; set; }
    public decimal CustoTransacao { get; set; }
    public StatusTransacao Status { get; set; }
    public string TID { get; set; } = string.Empty;
    public string NSU { get; set; } = string.Empty;

    public Guid PagamentoId { get; set; }
    public Pagamento? Pagamento { get; set; }
}