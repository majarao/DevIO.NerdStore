using DevIO.NerdStore.BFF.Compras.Models;
using DevIO.NerdStore.Core.Communication;

namespace DevIO.NerdStore.BFF.Compras.Services;

public interface ICarrinhoService
{
    Task<CarrinhoDTO?> ObterCarrinho();

    Task<ResponseResult?> AdicionarItemCarrinho(ItemCarrinhoDTO produto);

    Task<ResponseResult?> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO carrinho);

    Task<ResponseResult?> RemoverItemCarrinho(Guid produtoId);
}
