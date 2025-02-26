using DevIO.NerdStore.Catalogo.API.Models;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.Catalogo.API.Controllers;

public class CatalogoController(IProdutoRepository repository) : MainController
{
    private IProdutoRepository Repository { get; } = repository;

    [HttpGet("catalogo/produtos")]
    public async Task<IEnumerable<Produto>> Index() => await Repository.ObterTodos();

    [HttpGet("catalogo/produtos/{id}")]
    public async Task<Produto?> ProdutoDetalhe(Guid id) => await Repository.ObterPorId(id);

    [HttpGet("catalogo/produtos/lista/{ids}")]
    public async Task<IEnumerable<Produto>> ObterProdutosPorId(string ids) => await Repository.ObterProdutosPorId(ids);
}
