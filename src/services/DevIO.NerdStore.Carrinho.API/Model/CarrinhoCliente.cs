using FluentValidation;
using FluentValidation.Results;

namespace DevIO.NerdStore.Carrinho.API.Model;

public class CarrinhoCliente
{
    internal const int MAX_QUANTIDADE_ITEM = 5;

    public Guid Id { get; private set; }
    public Guid ClienteId { get; private set; }
    public decimal ValorTotal { get; private set; }
    public List<CarrinhoItem> Itens { get; private set; } = [];
    public ValidationResult? ValidationResult { get; private set; }

    protected CarrinhoCliente() { }

    public CarrinhoCliente(Guid clienteId)
    {
        Id = Guid.NewGuid();
        ClienteId = clienteId;
    }

    internal void AdicionarItem(CarrinhoItem item)
    {
        item.AssociarCarrinho(Id);

        if (CarrinhoItemExistente(item))
        {
            CarrinhoItem itemExistente = ObterPorProdutoId(item.ProdutoId);
            itemExistente.AdicionarUnidades(item.Quantidade);

            item = itemExistente;

            Itens.Remove(itemExistente);
        }

        Itens.Add(item);

        CalcularValorCarrinho();
    }

    internal bool CarrinhoItemExistente(CarrinhoItem item) => Itens.Any(p => p.ProdutoId == item?.ProdutoId);

    internal CarrinhoItem ObterPorProdutoId(Guid produtoId) => Itens.FirstOrDefault(p => p.ProdutoId == produtoId)!;

    internal void AtualizarUnidades(CarrinhoItem item, int unidades)
    {
        item.AtualizarUnidades(unidades);

        AtualizarItem(item);
    }

    internal void AtualizarItem(CarrinhoItem item)
    {
        item.AssociarCarrinho(Id);

        CarrinhoItem itemExistente = ObterPorProdutoId(item.ProdutoId);

        Itens.Remove(itemExistente);

        Itens.Add(item);

        CalcularValorCarrinho();
    }

    internal void RemoverItem(CarrinhoItem item)
    {
        Itens.Remove(ObterPorProdutoId(item.ProdutoId));

        CalcularValorCarrinho();
    }

    internal void CalcularValorCarrinho() => ValorTotal = Itens.Sum(p => p.CalcularValor());

    internal bool EhValido()
    {
        List<ValidationFailure> erros = Itens.SelectMany(i => new CarrinhoItem.ItemCarrinhoValidation().Validate(i).Errors).ToList();
        erros.AddRange(new CarrinhoClienteValidation().Validate(this).Errors);
        ValidationResult = new ValidationResult(erros);

        return ValidationResult.IsValid;
    }

    public class CarrinhoClienteValidation : AbstractValidator<CarrinhoCliente>
    {
        public CarrinhoClienteValidation()
        {
            RuleFor(c => c.ClienteId)
                .NotEqual(Guid.Empty)
                .WithMessage("Cliente não reconhecido");

            RuleFor(c => c.Itens.Count)
                .GreaterThan(0)
                .WithMessage("O carrinho não possui itens");

            RuleFor(c => c.ValorTotal)
                .GreaterThan(0)
                .WithMessage("O valor total do carrinho precisa ser maior que 0");
        }
    }
}
