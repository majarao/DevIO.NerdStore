using System.Text.Json.Serialization;

namespace DevIO.NerdStore.Carrinho.API.Model;

public class CarrinhoItem
{
    public Guid Id { get; set; }
    public Guid ProdutoId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public int Quantidade { get; set; }
    public decimal Valor { get; set; }
    public string Imagem { get; set; } = string.Empty;
    public Guid CarrinhoId { get; set; }

    public CarrinhoItem() =>
        Id = Guid.NewGuid();

    [JsonIgnore]
    public CarrinhoCliente? CarrinhoCliente { get; set; }
}
