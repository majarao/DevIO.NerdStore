using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebApp.MVC.Models;
using DevIO.NerdStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Controllers;

public class PedidoController(IClienteService clienteService, IComprasBFFService comprasBffService) : MainController
{
    private IClienteService ClienteService { get; } = clienteService;
    private IComprasBFFService ComprasBffService { get; } = comprasBffService;

    [HttpGet]
    [Route("endereco-de-entrega")]
    public async Task<IActionResult> EnderecoEntrega()
    {
        CarrinhoViewModel? carrinho = await ComprasBffService.ObterCarrinho();

        if (carrinho?.Itens.Count == 0 || carrinho is null)
            return RedirectToAction("Index", "Carrinho");

        EnderecoViewModel? endereco = await ClienteService.ObterEndereco();

        PedidoTransacaoViewModel pedido = ComprasBffService.MapearParaPedido(carrinho!, endereco);

        return View(pedido);
    }

    [HttpGet]
    [Route("pagamento")]
    public async Task<IActionResult> Pagamento()
    {
        CarrinhoViewModel? carrinho = await ComprasBffService.ObterCarrinho();

        if (carrinho?.Itens.Count == 0)
            return RedirectToAction("Index", "Carrinho");

        PedidoTransacaoViewModel pedido = ComprasBffService.MapearParaPedido(carrinho!, null);

        return View(pedido);
    }

    [HttpPost]
    [Route("finalizar-pedido")]
    public async Task<IActionResult> FinalizarPedido(PedidoTransacaoViewModel pedidoTransacao)
    {
        if (!ModelState.IsValid)
        {
            CarrinhoViewModel? carrinho = await ComprasBffService.ObterCarrinho();

            return View("Pagamento", ComprasBffService.MapearParaPedido(carrinho!, null));
        }

        ResponseResult? retorno = await ComprasBffService.FinalizarPedido(pedidoTransacao);

        if (ResponsePossuiErros(retorno))
        {
            CarrinhoViewModel? carrinho = await ComprasBffService.ObterCarrinho();

            if (carrinho?.Itens.Count == 0)
                return RedirectToAction("Index", "Carrinho");

            PedidoTransacaoViewModel pedidoMap = ComprasBffService.MapearParaPedido(carrinho!, null);

            return View("Pagamento", pedidoMap);
        }

        return RedirectToAction("PedidoConcluido");
    }

    [HttpGet]
    [Route("pedido-concluido")]
    public async Task<IActionResult> PedidoConcluido() => View("ConfirmacaoPedido", await ComprasBffService.ObterUltimoPedido());

    [HttpGet("meus-pedidos")]
    public async Task<IActionResult> MeusPedidos() => View(await ComprasBffService.ObterListaPorClienteId());
}
