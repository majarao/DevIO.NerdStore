using DevIO.NerdStore.Core.DomainObjects;
using DevIO.NerdStore.Pedidos.Domain.Vouchers;

namespace DevIO.NerdStore.Pedidos.Domain.Pedidos;

public class Pedido : Entity, IAggregateRoot
{
    public int Codigo { get; private set; }
    public Guid ClienteId { get; private set; }
    public Guid? VoucherId { get; private set; }
    public bool VoucherUtilizado { get; private set; }
    public decimal Desconto { get; private set; }
    public decimal ValorTotal { get; private set; }
    public DateTime DataCadastro { get; private set; }
    public PedidoStatus PedidoStatus { get; private set; }

    private readonly List<PedidoItem> _pedidoItems = [];
    public IReadOnlyCollection<PedidoItem> PedidoItems => _pedidoItems;

    public Endereco? Endereco { get; private set; }
    public Voucher? Voucher { get; private set; }

    protected Pedido() { }

    public Pedido(
        Guid clienteId,
        decimal valorTotal,
        List<PedidoItem> pedidoItems,
        bool voucherUtilizado = false,
        decimal desconto = 0,
        Guid? voucherId = null)
    {
        ClienteId = clienteId;
        ValorTotal = valorTotal;
        _pedidoItems = pedidoItems;
        Desconto = desconto;
        VoucherUtilizado = voucherUtilizado;
        VoucherId = voucherId;
    }

    public void AutorizarPedido() => PedidoStatus = PedidoStatus.Autorizado;

    public void AtribuirVoucher(Voucher voucher)
    {
        VoucherUtilizado = true;
        VoucherId = voucher.Id;
        Voucher = voucher;
    }

    public void AtribuirEndereco(Endereco endereco) => Endereco = endereco;

    public void CalcularValorPedido()
    {
        ValorTotal = PedidoItems.Sum(p => p.CalcularValor());
        CalcularValorTotalDesconto();
    }

    public void CalcularValorTotalDesconto()
    {
        if (!VoucherUtilizado)
            return;

        decimal desconto = 0;
        decimal valor = ValorTotal;

        if (Voucher?.TipoDesconto == TipoDescontoVoucher.Porcentagem)
        {
            if (Voucher.Percentual > 0)
            {
                desconto = valor * Voucher.Percentual / 100;
                valor -= desconto;
            }
        }
        else
        {
            if (Voucher?.ValorDesconto > 0)
            {
                desconto = Voucher.ValorDesconto;
                valor -= desconto;
            }
        }

        ValorTotal = valor < 0 ? 0 : valor;
        Desconto = desconto;
    }
}