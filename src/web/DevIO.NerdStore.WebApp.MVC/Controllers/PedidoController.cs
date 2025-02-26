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
}
