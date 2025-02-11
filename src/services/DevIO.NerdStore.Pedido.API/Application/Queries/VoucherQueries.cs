﻿using DevIO.NerdStore.Pedido.API.Application.DTO;
using DevIO.NerdStore.Pedido.Domain.Vouchers;

namespace DevIO.NerdStore.Pedido.API.Application.Queries;

public class VoucherQueries(IVoucherRepository repository) : IVoucherQueries
{
    private IVoucherRepository Repository { get; } = repository;

    public async Task<VoucherDTO?> ObterVoucherPorCodigo(string codigo)
    {
        Voucher? voucher = await Repository.ObterVoucherPorCodigo(codigo);

        if (voucher is null)
            return null;

        if (!voucher.EstaValidoParaUtilizacao())
            return null;

        return new VoucherDTO
        {
            Codigo = voucher.Codigo,
            TipoDesconto = (int)voucher.TipoDesconto,
            Percentual = voucher.Percentual,
            ValorDesconto = voucher.ValorDesconto
        };
    }
}
