using DevIO.NerdStore.Catalogo.API.Models;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.Catalogo.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
public class CatalogoController(IProdutoRepository repository) : MainController
{
    private IProdutoRepository ProdutoRepository { get; } = repository;

    [AllowAnonymous]
    [HttpGet("catalogo/produtos")]
    public async Task<IEnumerable<Produto>> Index() =>
        await ProdutoRepository.ObterTodos();

    [AllowAnonymous]
    [HttpGet("catalogo/produtos/{id}")]
    public async Task<Produto?> ProdutoDetalhe(Guid id) =>
        await ProdutoRepository.ObterPorId(id);
}
