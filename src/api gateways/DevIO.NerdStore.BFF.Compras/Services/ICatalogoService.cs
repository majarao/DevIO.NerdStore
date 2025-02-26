﻿using DevIO.NerdStore.BFF.Compras.Models;

namespace DevIO.NerdStore.BFF.Compras.Services;

public interface ICatalogoService
{
    Task<ItemProdutoDTO?> ObterPorId(Guid id);

    Task<IEnumerable<ItemProdutoDTO>> ObterItens(IEnumerable<Guid>? ids);
}
