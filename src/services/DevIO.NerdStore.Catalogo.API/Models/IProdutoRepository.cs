﻿using DevIO.NerdStore.Core.Data;

namespace DevIO.NerdStore.Catalogo.API.Models;

public interface IProdutoRepository : IRepository<Produto>
{
    Task<PagedResult<Produto>> ObterTodos(int pageSize, int pageIndex, string? query = null);

    Task<Produto?> ObterPorId(Guid id);

    void Adicionar(Produto produto);

    void Atualizar(Produto produto);

    Task<List<Produto>> ObterProdutosPorId(string ids);
}
