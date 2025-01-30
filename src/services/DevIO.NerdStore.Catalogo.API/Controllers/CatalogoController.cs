using DevIO.NerdStore.Catalogo.API.Models;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.Catalogo.API.Controllers;

public class CatalogoController(IProdutoRepository repository) : MainController
{
    private IProdutoRepository ProdutoRepository { get; } = repository;

    [HttpGet("catalogo/produtos")]
    public async Task<IEnumerable<Produto>> Index() =>
        await ProdutoRepository.ObterTodos();

    [HttpGet("catalogo/produtos/{id}")]
    public async Task<Produto?> ProdutoDetalhe(Guid id) =>
        await ProdutoRepository.ObterPorId(id);
}
