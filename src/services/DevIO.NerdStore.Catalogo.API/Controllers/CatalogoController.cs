using DevIO.NerdStore.Catalogo.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.Catalogo.API.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CatalogoController(IProdutoRepository repository) : Controller
{
    private IProdutoRepository Repository { get; } = repository;

    [HttpGet("catalogo/produtos")]
    public async Task<IEnumerable<Produto>> Index() =>
        await Repository.ObterTodos();

    [HttpGet("catalogo/produtos/{id}")]
    public async Task<Produto?> ProdutoDetalhe(Guid id) =>
        await Repository.ObterPorId(id);
}
