using DevIO.NerdStore.WebApp.MVC.Models;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public interface ICatalogoService
{
    Task<ProdutoViewModel?> ObterPorId(Guid id);

    Task<PagedViewModel<ProdutoViewModel>> ObterTodos(int pageSize, int pageIndex, string? query = null);
}
