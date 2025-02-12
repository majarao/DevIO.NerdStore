namespace DevIO.NerdStore.Carrinho.API.Model;

public class Voucher
{
    public decimal Percentual { get; set; }
    public decimal ValorDesconto { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public TipoDescontoVoucher TipoDesconto { get; set; }
}
