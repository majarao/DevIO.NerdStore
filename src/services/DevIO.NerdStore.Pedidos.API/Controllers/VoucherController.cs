using DevIO.NerdStore.Pedidos.API.Application.DTO;
using DevIO.NerdStore.Pedidos.API.Application.Queries;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevIO.NerdStore.Pedidos.API.Controllers;

[Authorize]
public class VoucherController(IVoucherQueries voucherQueries) : MainController
{
    private IVoucherQueries VoucherQueries { get; } = voucherQueries;

    [HttpGet("voucher/{codigo}")]
    [ProducesResponseType(typeof(VoucherDTO), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> ObterPorCodigo(string codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return NotFound();

        VoucherDTO? voucher = await VoucherQueries.ObterVoucherPorCodigo(codigo);

        return voucher is null ? NotFound() : CustomResponse(voucher);
    }
}
