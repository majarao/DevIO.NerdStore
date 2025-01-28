using FluentValidation.Results;

namespace DevIO.NerdStore.Carrinho.API.Model;

public class CarrinhoCliente
{
    public Guid Id { get; set; }
    public Guid ClienteId { get; set; }
    public decimal ValorTotal { get; set; }
    public List<CarrinhoItem> Itens { get; set; } = [];
    public ValidationResult? ValidationResult { get; set; }

    protected CarrinhoCliente() { }

    public CarrinhoCliente(Guid clienteId)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
    }
}
