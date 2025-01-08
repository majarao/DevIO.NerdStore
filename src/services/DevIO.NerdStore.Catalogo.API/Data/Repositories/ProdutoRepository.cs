using DevIO.NerdStore.Catalogo.API.Models;
using DevIO.NerdStore.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Catalogo.API.Data.Repositories;

public class ProdutoRepository(CatalogoContext catalogoContext) : IProdutoRepository
{
    private CatalogoContext CatalogoContext { get; } = catalogoContext;
    public IUnitOfWork UnitOfWork => CatalogoContext;

    public async Task<Produto?> ObterPorId(Guid id) => await CatalogoContext.Produtos.SingleOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Produto>> ObterTodos() => await CatalogoContext.Produtos.AsNoTracking().ToListAsync();

    public void Adicionar(Produto produto) => CatalogoContext.Produtos.Add(produto);

    public void Atualizar(Produto produto) => CatalogoContext.Produtos.Update(produto);

    public void Dispose() => CatalogoContext?.Dispose();
}
