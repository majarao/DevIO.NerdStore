using DevIO.NerdStore.WebApp.MVC.Models;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public interface ICatalogoService
{
    Task<IEnumerable<ProdutoViewModel>?> ObterTodos();

    Task<ProdutoViewModel?> ObterPorId(Guid id);
}
