namespace DevIO.NerdStore.WebApp.MVC.Models;

public class CarrinhoViewModel
{
    public decimal ValorTotal { get; set; }
    public List<ItemCarrinhoViewModel> Itens { get; set; } = [];
    public VoucherViewModel? Voucher { get; set; }
    public bool VoucherUtilizado { get; set; }
    public decimal Desconto { get; set; }
}
