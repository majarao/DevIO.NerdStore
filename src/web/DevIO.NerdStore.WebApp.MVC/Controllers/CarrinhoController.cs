using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebApp.MVC.Models;
using DevIO.NerdStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Controllers;

[Authorize]
public class CarrinhoController(ICarrinhoService carrinhoService, ICatalogoService catalogoService) : MainController
{
    private ICarrinhoService CarrinhoService { get; } = carrinhoService;
    private ICatalogoService CatalogoService { get; } = catalogoService;

    [Route("carrinho")]
    public async Task<IActionResult> Index() => View(await CarrinhoService.ObterCarrinho());

    [HttpPost]
    [Route("carrinho/adicionar-item")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemProdutoViewModel itemProduto)
    {
        ProdutoViewModel? produto = await CatalogoService.ObterPorId(itemProduto.ProdutoId);

        ValidarItemCarrinho(produto, itemProduto.Quantidade);

        if (!OperacaoValida())
            return View("Index", await CarrinhoService.ObterCarrinho());

        itemProduto.Nome = produto!.Nome;
        itemProduto.Valor = produto.Valor;
        itemProduto.Imagem = produto.Imagem;

        ResponseResult? resposta = await CarrinhoService.AdicionarItemCarrinho(itemProduto);

        if (ResponsePossuiErros(resposta))
            return View("Index", await CarrinhoService.ObterCarrinho());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("carrinho/atualizar-item")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, int quantidade)
    {
        ProdutoViewModel? produto = await CatalogoService.ObterPorId(produtoId);

        ValidarItemCarrinho(produto, quantidade);

        if (!OperacaoValida())
            return View("Index", await CarrinhoService.ObterCarrinho());

        ItemProdutoViewModel itemProduto = new() { ProdutoId = produtoId, Quantidade = quantidade };

        ResponseResult? resposta = await CarrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);

        if (ResponsePossuiErros(resposta))
            return View("Index", await CarrinhoService.ObterCarrinho());

        return RedirectToAction("Index");
    }

    [HttpPost]
    [Route("carrinho/remover-item")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        ProdutoViewModel? produto = await CatalogoService.ObterPorId(produtoId);

        if (produto is null)
        {
            AdicionarErroValidacao("Produto inexistente!");

            return View("Index", await CarrinhoService.ObterCarrinho());
        }

        ResponseResult? resposta = await CarrinhoService.RemoverItemCarrinho(produtoId);

        if (ResponsePossuiErros(resposta))
            return View("Index", await CarrinhoService.ObterCarrinho());

        return RedirectToAction("Index");
    }

    private void ValidarItemCarrinho(ProdutoViewModel? produto, int quantidade)
    {
        if (produto is null)
            AdicionarErroValidacao("Produto inexistente!");

        if (quantidade < 1)
            AdicionarErroValidacao($"Escolha ao menos uma unidade do produto {produto!.Nome}");

        if (quantidade > produto!.QuantidadeEstoque)
            AdicionarErroValidacao($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
    }
}
