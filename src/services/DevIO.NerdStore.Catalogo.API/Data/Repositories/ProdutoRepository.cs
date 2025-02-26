using DevIO.NerdStore.Catalogo.API.Models;
using DevIO.NerdStore.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Catalogo.API.Data.Repositories;

public class ProdutoRepository(CatalogoContext context) : IProdutoRepository
{
    private CatalogoContext Context { get; } = context;
    public IUnitOfWork UnitOfWork => Context;

    public async Task<Produto?> ObterPorId(Guid id) => await Context.Produtos.SingleOrDefaultAsync(p => p.Id == id);

    public async Task<IEnumerable<Produto>> ObterTodos() => await Context.Produtos.AsNoTracking().ToListAsync();

    public void Adicionar(Produto produto) => Context.Produtos.Add(produto);

    public void Atualizar(Produto produto) => Context.Produtos.Update(produto);

    public async Task<List<Produto>> ObterProdutosPorId(string ids)
    {
        IEnumerable<(bool Ok, Guid Value)> idsGuid = ids.Split(',').Select(id => (Ok: Guid.TryParse(id, out Guid x), Value: x));

        if (!idsGuid.All(nid => nid.Ok))
            return [];

        IEnumerable<Guid> idsValue = idsGuid.Select(id => id.Value);

        return await Context.Produtos.AsNoTracking().Where(p => idsValue.Contains(p.Id) && p.Ativo).ToListAsync();
    }

    public void Dispose() => GC.SuppressFinalize(this);
}
