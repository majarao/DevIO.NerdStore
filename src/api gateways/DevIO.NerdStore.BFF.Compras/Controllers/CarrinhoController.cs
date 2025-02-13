using DevIO.NerdStore.BFF.Compras.Models;
using DevIO.NerdStore.BFF.Compras.Services;
using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.BFF.Compras.Controllers;

[Authorize]
public class CarrinhoController(ICarrinhoService carrinhoService, ICatalogoService catalogoService, IPedidoService pedidoService) : MainController
{
    private ICarrinhoService CarrinhoService { get; } = carrinhoService;
    private ICatalogoService CatalogoService { get; } = catalogoService;
    private IPedidoService PedidoService { get; } = pedidoService;

    [HttpGet]
    [Route("compras/carrinho")]
    public async Task<IActionResult> Index() => CustomResponse(await CarrinhoService.ObterCarrinho());

    [HttpGet]
    [Route("compras/carrinho-quantidade")]
    public async Task<int> ObterQuantidadeCarrinho()
    {
        CarrinhoDTO? quantidade = await CarrinhoService.ObterCarrinho();

        return quantidade?.Itens.Sum(i => i.Quantidade) ?? 0;
    }

    [HttpPost]
    [Route("compras/carrinho/items")]
    public async Task<IActionResult> AdicionarItemCarrinho(ItemCarrinhoDTO itemProduto)
    {
        ItemProdutoDTO? produto = await CatalogoService.ObterPorId(itemProduto.ProdutoId);

        await ValidarItemCarrinho(produto, itemProduto.Quantidade, true);

        if (!OperacaoValida())
            return CustomResponse();

        itemProduto.Nome = produto?.Nome ?? string.Empty;
        itemProduto.Valor = produto?.Valor ?? 0;
        itemProduto.Imagem = produto?.Imagem ?? string.Empty;

        ResponseResult? resposta = await CarrinhoService.AdicionarItemCarrinho(itemProduto);

        return CustomResponse(resposta);
    }

    [HttpPut]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, ItemCarrinhoDTO itemProduto)
    {
        ItemProdutoDTO? produto = await CatalogoService.ObterPorId(produtoId);

        await ValidarItemCarrinho(produto, itemProduto.Quantidade);

        if (!OperacaoValida())
            return CustomResponse();

        ResponseResult? resposta = await CarrinhoService.AtualizarItemCarrinho(produtoId, itemProduto);

        return CustomResponse(resposta);
    }

    [HttpDelete]
    [Route("compras/carrinho/items/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        ItemProdutoDTO? produto = await CatalogoService.ObterPorId(produtoId);

        if (produto is null)
        {
            AdicionarErroProcessamento("Produto inexistente!");

            return CustomResponse();
        }

        ResponseResult? resposta = await CarrinhoService.RemoverItemCarrinho(produtoId);

        return CustomResponse(resposta);
    }

    [HttpPost]
    [Route("compras/carrinho/aplicar-voucher")]
    public async Task<IActionResult> AplicarVoucher([FromBody] string voucherCodigo)
    {
        VoucherDTO? voucher = await PedidoService.ObterVoucherPorCodigo(voucherCodigo);

        if (voucher is null)
        {
            AdicionarErroProcessamento("Voucher inválido ou não encontrado!");
            return CustomResponse();
        }

        ResponseResult? resposta = await CarrinhoService.AplicarVoucherCarrinho(voucher);

        return CustomResponse(resposta);
    }

    private async Task ValidarItemCarrinho(ItemProdutoDTO? produto, int quantidade, bool adicionarProduto = false)
    {
        if (produto is null)
            AdicionarErroProcessamento("Produto inexistente!");

        if (quantidade < 1)
            AdicionarErroProcessamento($"Escolha ao menos uma unidade do produto {produto?.Nome}");

        CarrinhoDTO? carrinho = await CarrinhoService.ObterCarrinho();

        ItemCarrinhoDTO? itemCarrinho = carrinho?.Itens.FirstOrDefault(p => p.ProdutoId == produto?.Id);

        if (itemCarrinho is not null && adicionarProduto && itemCarrinho.Quantidade + quantidade > produto?.QuantidadeEstoque)
        {
            AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
            return;
        }

        if (quantidade > produto?.QuantidadeEstoque)
            AdicionarErroProcessamento($"O produto {produto.Nome} possui {produto.QuantidadeEstoque} unidades em estoque, você selecionou {quantidade}");
    }
}
