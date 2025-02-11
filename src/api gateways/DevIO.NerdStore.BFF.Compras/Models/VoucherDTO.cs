namespace DevIO.NerdStore.BFF.Compras.Models;

public class VoucherDTO
{
    public decimal Percentual { get; set; }
    public decimal ValorDesconto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public int TipoDesconto { get; set; }
}
