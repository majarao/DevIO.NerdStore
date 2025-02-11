namespace DevIO.NerdStore.Pedido.API.Application.DTO;

public class VoucherDTO
{
    public string Codigo { get; set; } = string.Empty;
    public decimal Percentual { get; set; }
    public decimal ValorDesconto { get; set; }
    public int TipoDesconto { get; set; }
}
