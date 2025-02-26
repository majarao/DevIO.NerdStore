using DevIO.NerdStore.BFF.Compras.Models;
using DevIO.NerdStore.BFF.Compras.Services;
using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DevIO.NerdStore.BFF.Compras.Controllers;

[Authorize]
public class PedidoController(
    ICatalogoService catalogoService,
    ICarrinhoService carrinhoService,
    IPedidoService pedidoService,
    IClienteService clienteService) : MainController
{
    private ICatalogoService CatalogoService { get; } = catalogoService;
    private ICarrinhoService CarrinhoService { get; } = carrinhoService;
    private IPedidoService PedidoService { get; } = pedidoService;
    private IClienteService ClienteService { get; } = clienteService;

    [HttpPost]
    [Route("compras/pedido")]
    public async Task<IActionResult> AdicionarPedido(PedidoDTO pedido)
    {
        CarrinhoDTO? carrinho = await CarrinhoService.ObterCarrinho();

        IEnumerable<ItemProdutoDTO> produtos = await CatalogoService.ObterItens(carrinho?.Itens.Select(p => p.ProdutoId));

        EnderecoDTO? endereco = await ClienteService.ObterEndereco();

        if (!await ValidarCarrinhoProdutos(carrinho!, produtos))
            return CustomResponse();

        PopularDadosPedido(carrinho!, endereco, pedido);

        return CustomResponse(await PedidoService.FinalizarPedido(pedido));
    }

    [HttpGet("compras/pedido/ultimo")]
    public async Task<IActionResult> UltimoPedido()
    {
        PedidoDTO? pedido = await PedidoService.ObterUltimoPedido();

        if (pedido is null)
        {
            AdicionarErroProcessamento("Pedido não encontrado!");

            return CustomResponse();
        }

        return CustomResponse(pedido);
    }

    [HttpGet("compras/pedido/lista-cliente")]
    public async Task<IActionResult> ListaPorCliente()
    {
        IEnumerable<PedidoDTO> pedidos = await PedidoService.ObterListaPorClienteId();

        return pedidos == null ? NotFound() : CustomResponse(pedidos);
    }

    private async Task<bool> ValidarCarrinhoProdutos(CarrinhoDTO carrinho, IEnumerable<ItemProdutoDTO> produtos)
    {
        if (carrinho.Itens.Count != produtos.Count())
        {
            List<Guid> itensIndisponiveis = carrinho.Itens.Select(c => c.ProdutoId).Except(produtos.Select(p => p.Id)).ToList();

            foreach (Guid itemId in itensIndisponiveis)
            {
                ItemCarrinhoDTO? itemCarrinho = carrinho?.Itens.FirstOrDefault(c => c?.ProdutoId == itemId);

                AdicionarErroProcessamento($"O item {itemCarrinho?.Nome} não está mais disponível no catálogo, o remova do carrinho para prosseguir com a compra");
            }

            return false;
        }

        foreach (ItemCarrinhoDTO itemCarrinho in carrinho.Itens)
        {
            ItemProdutoDTO? produtoCatalogo = produtos?.FirstOrDefault(p => p?.Id == itemCarrinho?.ProdutoId);

            if (produtoCatalogo?.Valor != itemCarrinho.Valor)
            {
                string msgErro = $"O produto {itemCarrinho.Nome} mudou de valor (de: " +
                              $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", itemCarrinho.Valor)} para: " +
                              $"{string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", produtoCatalogo?.Valor)}) desde que foi adicionado ao carrinho.";

                AdicionarErroProcessamento(msgErro);

                ResponseResult? responseRemover = await CarrinhoService.RemoverItemCarrinho(itemCarrinho.ProdutoId);

                if (ResponsePossuiErros(responseRemover))
                {
                    AdicionarErroProcessamento($"Não foi possível remover automaticamente o produto {itemCarrinho?.Nome} do seu carrinho, _" +
                                               "remova e adicione novamente caso ainda deseje comprar este item");

                    return false;
                }

                itemCarrinho.Valor = produtoCatalogo?.Valor ?? 0;

                ResponseResult? responseAdicionar = await CarrinhoService.AdicionarItemCarrinho(itemCarrinho);

                if (ResponsePossuiErros(responseAdicionar))
                {
                    AdicionarErroProcessamento($"Não foi possível atualizar automaticamente o produto {itemCarrinho?.Nome} do seu carrinho, _" +
                                               "adicione novamente caso ainda deseje comprar este item");

                    return false;
                }

                LimparErrosProcessamento();

                AdicionarErroProcessamento(msgErro + " Atualizamos o valor em seu carrinho, realize a conferência do pedido e se preferir remova o produto");

                return false;
            }
        }

        return true;
    }

    private static void PopularDadosPedido(CarrinhoDTO carrinho, EnderecoDTO? endereco, PedidoDTO pedido)
    {
        pedido.VoucherCodigo = carrinho.Voucher?.Codigo;
        pedido.VoucherUtilizado = carrinho.VoucherUtilizado;
        pedido.ValorTotal = carrinho.ValorTotal;
        pedido.Desconto = carrinho.Desconto;
        pedido.PedidoItems = carrinho.Itens;
        pedido.Endereco = endereco;
    }
}
