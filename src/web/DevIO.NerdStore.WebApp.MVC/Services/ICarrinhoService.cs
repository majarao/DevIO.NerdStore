using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebApp.MVC.Models;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public interface ICarrinhoService
{
    Task<CarrinhoViewModel?> ObterCarrinho();

    Task<ResponseResult?> AdicionarItemCarrinho(ItemProdutoViewModel produto);

    Task<ResponseResult?> AtualizarItemCarrinho(Guid produtoId, ItemProdutoViewModel produto);

    Task<ResponseResult?> RemoverItemCarrinho(Guid produtoId);
}
