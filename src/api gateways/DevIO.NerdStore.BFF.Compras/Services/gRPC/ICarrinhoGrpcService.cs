using DevIO.NerdStore.BFF.Compras.Models;

namespace DevIO.NerdStore.BFF.Compras.Services.gRPC;

public interface ICarrinhoGrpcService
{
    Task<CarrinhoDTO> ObterCarrinho();
}
