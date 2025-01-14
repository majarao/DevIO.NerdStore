using DevIO.NerdStore.WebApp.MVC.Models;
using DevIO.NerdStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Controllers;

public class CatalogoController(ICatalogoService catalogoService) : MainController
{
    private ICatalogoService CatalogoService { get; } = catalogoService;

    [HttpGet]
    [Route("")]
    [Route("vitrine")]
    public async Task<IActionResult> Index()
    {
        IEnumerable<ProdutoViewModel>? produtos = await CatalogoService.ObterTodos();

        return View(produtos);
    }

    [HttpGet]
    [Route("produto-detalhe/{id}")]
    public async Task<IActionResult> ProdutoDetalhe(Guid id)
    {
        ProdutoViewModel? produto = await CatalogoService.ObterPorId(id);

        return View(produto);
    }
}
