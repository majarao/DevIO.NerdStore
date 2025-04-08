using DevIO.NerdStore.Carrinho.API.Data;
using DevIO.NerdStore.Carrinho.API.Model;
using DevIO.NerdStore.WebAPI.Core.Usuario;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Carrinho.API.Services.gRPC;

[Authorize]
public class CarrinhoGrpcService(IAspNetUser user, CarrinhoContext context) : CarrinhoCompras.CarrinhoComprasBase
{
    private IAspNetUser User { get; } = user;
    private CarrinhoContext Context { get; } = context;

    public override async Task<CarrinhoClienteResponse> ObterCarrinho(ObterCarrinhoRequest request, ServerCallContext context)
    {
        CarrinhoCliente carrinho = await ObterCarrinhoCliente() ?? new();

        return MapCarrinhoClienteToProtoResponse(carrinho);
    }

    private async Task<CarrinhoCliente?> ObterCarrinhoCliente() => 
        await Context.CarrinhoCliente
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.ClienteId == User.ObterUserId());

    private static CarrinhoClienteResponse MapCarrinhoClienteToProtoResponse(CarrinhoCliente carrinho)
    {
        CarrinhoClienteResponse carrinhoProto = new()
        {
            Id = carrinho.Id.ToString(),
            Clienteid = carrinho.ClienteId.ToString(),
            Valortotal = (double)carrinho.ValorTotal,
            Desconto = (double)carrinho.Desconto,
            Voucherutilizado = carrinho.VoucherUtilizado,
        };

        if (carrinho.Voucher is not null)
        {
            carrinhoProto.Voucher = new()
            {
                Codigo = carrinho.Voucher.Codigo,
                Percentual = (double)carrinho.Voucher.Percentual,
                Valordesconto = (double)carrinho.Voucher.ValorDesconto,
                Tipodesconto = (int)carrinho.Voucher.TipoDesconto
            };
        }

        foreach (CarrinhoItem item in carrinho.Itens)
        {
            carrinhoProto.Itens.Add(new CarrinhoItemResponse
            {
                Id = item.Id.ToString(),
                Nome = item.Nome,
                Imagem = item.Imagem,
                Produtoid = item.ProdutoId.ToString(),
                Quantidade = item.Quantidade,
                Valor = (double)item.Valor
            });
        }

        return carrinhoProto;
    }
}
