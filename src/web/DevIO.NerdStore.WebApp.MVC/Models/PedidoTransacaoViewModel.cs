namespace DevIO.NerdStore.WebApp.MVC.Models;

public class PedidoTransacaoViewModel
{
    public decimal ValorTotal { get; set; }
    public decimal Desconto { get; set; }
    public string? VoucherCodigo { get; set; }
    public bool VoucherUtilizado { get; set; }

    public List<ItemCarrinhoViewModel> Itens { get; set; } = [];

    public EnderecoViewModel? Endereco { get; set; }
}
