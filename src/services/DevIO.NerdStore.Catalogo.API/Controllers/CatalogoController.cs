using DevIO.NerdStore.Catalogo.API.Models;
using DevIO.NerdStore.WebAPI.Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.Catalogo.API.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "Bearer")]
public class CatalogoController(IProdutoRepository repository) : Controller
{
    private IProdutoRepository Repository { get; } = repository;

    [AllowAnonymous]
    [HttpGet("catalogo/produtos")]
    public async Task<IEnumerable<Produto>> Index() =>
        await Repository.ObterTodos();

    [ClaimsAuthorize("Catalogo", "Ler")]
    [HttpGet("catalogo/produtos/{id}")]
    public async Task<Produto?> ProdutoDetalhe(Guid id) =>
        await Repository.ObterPorId(id);
}
