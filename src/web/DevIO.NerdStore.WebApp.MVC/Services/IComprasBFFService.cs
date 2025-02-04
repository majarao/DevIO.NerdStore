using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebApp.MVC.Models;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public interface IComprasBFFService
{
    Task<CarrinhoViewModel?> ObterCarrinho();

    Task<int> ObterQuantidadeCarrinho();

    Task<ResponseResult?> AdicionarItemCarrinho(ItemCarrinhoViewModel produto);

    Task<ResponseResult?> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoViewModel produto);

    Task<ResponseResult?> RemoverItemCarrinho(Guid produtoId);
}
