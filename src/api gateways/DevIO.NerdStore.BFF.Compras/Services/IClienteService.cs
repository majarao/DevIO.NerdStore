﻿using DevIO.NerdStore.BFF.Compras.Models;

namespace DevIO.NerdStore.BFF.Compras.Services;

public interface IClienteService
{
    Task<EnderecoDTO?> ObterEndereco();
}
