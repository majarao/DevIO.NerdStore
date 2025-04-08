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
    public async Task<IActionResult> Index([FromQuery] int ps = 8, [FromQuery] int page = 1, [FromQuery] string? q = null)
    {
        PagedViewModel<ProdutoViewModel> produtos = await CatalogoService.ObterTodos(ps, page, q);

        ViewBag.Pesquisa = q;

        produtos.ReferenceAction = "Index";

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
