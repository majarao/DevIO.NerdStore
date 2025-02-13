using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebApp.MVC.Models;
using DevIO.NerdStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Controllers;

[Authorize]
public class CarrinhoController(IComprasBFFService carrinhoService) : MainController
{
    private IComprasBFFService BFFService { get; } = carrinhoService;

    [Route("carrinho")]
    public async Task<IActionResult> Index() => View(await BFFService.ObterCarrinho());

    [HttpPost]
    [Route("carrinho/adicionar-item")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoViewModel itemCarrinho)
    {
        ResponseResult? resposta = await BFFService.AdicionarItemCarrinho(itemCarrinho);

        if (ResponsePossuiErros(resposta))
            return View("Index", await BFFService.ObterCarrinho());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("carrinho/atualizar-item")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
    {
        ItemCarrinhoViewModel item = new() { ProdutoId = produtoId, Quantidade = quantidade };

        ResponseResult? resposta = await BFFService.AtualizarItemCarrinho(produtoId, item);

        if (ResponsePossuiErros(resposta))
            return View("Index", await BFFService.ObterCarrinho());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("carrinho/remover-item")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        ResponseResult? resposta = await BFFService.RemoverItemCarrinho(produtoId);

        if (ResponsePossuiErros(resposta))
            return View("Index", await BFFService.ObterCarrinho());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("carrinho/aplicar-voucher")]
    public async Task<IActionResult> AplicarVoucher(string voucherCodigo)
    {
        ResponseResult? resposta = await BFFService.AplicarVoucherCarrinho(voucherCodigo);

        if (ResponsePossuiErros(resposta))
            return View("Index", await BFFService.ObterCarrinho());

        return RedirectToAction("Index");
    }
}
