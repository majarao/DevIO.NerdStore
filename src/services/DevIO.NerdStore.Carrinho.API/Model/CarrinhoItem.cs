using FluentValidation;
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
    public CarrinhoCliente? CarrinhoCliente { get; set; }

    [JsonConstructor]
    protected CarrinhoItem() => Id = Guid.NewGuid();

    internal void AssociarCarrinho(Guid carrinhoId) => CarrinhoId = carrinhoId;

    internal void AdicionarUnidades(int unidades) => Quantidade += unidades;

    internal void AtualizarUnidades(int unidades) => Quantidade = unidades;

    internal decimal CalcularValor() => Quantidade * Valor;

    internal bool EhValido() => new ItemCarrinhoValidation().Validate(this).IsValid;

    public class ItemCarrinhoValidation : AbstractValidator<CarrinhoItem>
    {
        public ItemCarrinhoValidation()
        {
            RuleFor(c => c.ProdutoId)
                .NotEqual(Guid.Empty)
                .WithMessage("Id do produto inválido");

            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("O nome do produto não foi informado");

            RuleFor(c => c.Quantidade)
                .GreaterThan(0)
                .WithMessage(item => $"A quantidade miníma para o {item.Nome} é 1");

            RuleFor(c => c.Quantidade)
                .LessThanOrEqualTo(CarrinhoCliente.MAX_QUANTIDADE_ITEM)
                .WithMessage(item => $"A quantidade máxima do {item.Nome} é {CarrinhoCliente.MAX_QUANTIDADE_ITEM}");

            RuleFor(c => c.Valor)
                .GreaterThan(0)
                .WithMessage(item => $"O valor do {item.Nome} precisa ser maior que 0");
        }
    }
}
