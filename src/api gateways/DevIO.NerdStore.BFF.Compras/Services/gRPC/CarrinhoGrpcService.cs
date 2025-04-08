using DevIO.NerdStore.BFF.Compras.Models;
using DevIO.NerdStore.Carrinho.API.Services.gRPC;

namespace DevIO.NerdStore.BFF.Compras.Services.gRPC;

public class CarrinhoGrpcService(CarrinhoCompras.CarrinhoComprasClient carrinhoComprasClient) : ICarrinhoGrpcService
{
    private CarrinhoCompras.CarrinhoComprasClient Client { get; } = carrinhoComprasClient;

    public async Task<CarrinhoDTO> ObterCarrinho()
    {
        CarrinhoClienteResponse response = await Client.ObterCarrinhoAsync(new ObterCarrinhoRequest());

        return MapCarrinhoClienteProtoResponseToDTO(response);
    }

    private static CarrinhoDTO MapCarrinhoClienteProtoResponseToDTO(CarrinhoClienteResponse carrinhoResponse)
    {
        CarrinhoDTO carrinhoDTO = new()
        {
            ValorTotal = (decimal)carrinhoResponse.Valortotal,
            Desconto = (decimal)carrinhoResponse.Desconto,
            VoucherUtilizado = carrinhoResponse.Voucherutilizado
        };

        if (carrinhoResponse.Voucher is not null)
        {
            carrinhoDTO.Voucher = new()
            {
                Codigo = carrinhoResponse.Voucher.Codigo,
                Percentual = (decimal)carrinhoResponse.Voucher.Percentual,
                ValorDesconto = (decimal)carrinhoResponse.Voucher.Valordesconto,
                TipoDesconto = carrinhoResponse.Voucher.Tipodesconto
            };
        }

        foreach (CarrinhoItemResponse? item in carrinhoResponse.Itens)
        {
            carrinhoDTO.Itens.Add(new ItemCarrinhoDTO
            {
                Nome = item.Nome,
                Imagem = item.Imagem,
                ProdutoId = Guid.Parse(item.Produtoid),
                Quantidade = item.Quantidade,
                Valor = (decimal)item.Valor
            });
        }

        return carrinhoDTO;
    }
}
