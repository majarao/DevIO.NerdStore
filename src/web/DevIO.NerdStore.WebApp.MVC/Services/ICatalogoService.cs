using DevIO.NerdStore.WebApp.MVC.Models;
using Refit;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public interface ICatalogoService
{
    [Get("/catalogo/produtos/")]
    Task<IEnumerable<ProdutoViewModel>?> ObterTodos();

    [Get("/catalogo/produtos/{id}")]
    Task<ProdutoViewModel?> ObterPorId(Guid id);
}
