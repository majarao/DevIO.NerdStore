using DevIO.NerdStore.Core.Messages;
using DevIO.NerdStore.Pedidos.API.Application.DTO;
using DevIO.NerdStore.Pedidos.API.Application.Events;
using DevIO.NerdStore.Pedidos.Domain.Pedidos;
using DevIO.NerdStore.Pedidos.Domain.Vouchers;
using DevIO.NerdStore.Pedidos.Domain.Vouchers.Specs;
using FluentValidation.Results;
using MediatR;

namespace DevIO.NerdStore.Pedidos.API.Application.Commands;

public class PedidoCommandHandler(IPedidoRepository pedidoRepository, IVoucherRepository voucherRepository) :
    CommandHandler,
    IRequestHandler<AdicionarPedidoCommand, ValidationResult?>
{
    private IPedidoRepository PedidoRepository { get; } = pedidoRepository;
    private IVoucherRepository VoucherRepository { get; } = voucherRepository;

    public async Task<ValidationResult?> Handle(AdicionarPedidoCommand message, CancellationToken cancellationToken)
    {
        if (!message.EhValido())
            return message.ValidationResult;

        Pedido pedido = MapearPedido(message);

        if (!await AplicarVoucher(message, pedido))
            return ValidationResult;

        if (!ValidarPedido(pedido))
            return ValidationResult;

        if (!ProcessarPagamento(pedido))
            return ValidationResult;

        pedido.AutorizarPedido();

        pedido.AdicionarEvento(new PedidoRealizadoEvent(pedido.Id, pedido.ClienteId));

        PedidoRepository.Adicionar(pedido);

        return await PersistirDados(PedidoRepository.UnitOfWork);
    }

    private static Pedido MapearPedido(AdicionarPedidoCommand message)
    {
        Endereco endereco = new()
        {
            Logradouro = message.Endereco?.Logradouro,
            Numero = message.Endereco?.Numero,
            Complemento = message.Endereco?.Complemento,
            Bairro = message.Endereco?.Bairro,
            Cep = message.Endereco?.Cep,
            Cidade = message.Endereco?.Cidade,
            Estado = message.Endereco?.Estado
        };

        Pedido pedido = new(
            message.ClienteId,
            message.ValorTotal,
            message.PedidoItems.Select(PedidoItemDTO.ParaPedidoItem).ToList(),
            message.VoucherUtilizado, message.Desconto);

        pedido.AtribuirEndereco(endereco);

        return pedido;
    }

    private async Task<bool> AplicarVoucher(AdicionarPedidoCommand message, Pedido pedido)
    {
        if (!message.VoucherUtilizado)
            return true;

        Voucher? voucher = await VoucherRepository.ObterVoucherPorCodigo(message.VoucherCodigo ?? string.Empty);
        if (voucher is null)
        {
            AdicionarErro("O voucher informado não existe!");
            return false;
        }

        ValidationResult voucherValidation = new VoucherValidation().Validate(voucher);
        if (!voucherValidation.IsValid)
        {
            voucherValidation.Errors.ToList().ForEach(m => AdicionarErro(m.ErrorMessage));
            return false;
        }

        pedido.AtribuirVoucher(voucher);

        voucher.DebitarQuantidade();

        VoucherRepository.Atualizar(voucher);

        return true;
    }

    private bool ValidarPedido(Pedido pedido)
    {
        decimal pedidoValorOriginal = pedido.ValorTotal;
        decimal pedidoDesconto = pedido.Desconto;

        pedido.CalcularValorPedido();

        if (pedido.ValorTotal != pedidoValorOriginal)
        {
            AdicionarErro("O valor total do pedido não confere com o cálculo do pedido");
            return false;
        }

        if (pedido.Desconto != pedidoDesconto)
        {
            AdicionarErro("O desconto não confere com o cálculo do pedido");
            return false;
        }

        return true;
    }

    public bool ProcessarPagamento(Pedido pedido)
    {
        return true;
    }
}
