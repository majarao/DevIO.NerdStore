using DevIO.NerdStore.Carrinho.API.Model;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using DevIO.NerdStore.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.Carrinho.API.Controllers;

[Authorize]
public class CarrinhoController(IAspNetUser user) : MainController
{
    private IAspNetUser User { get; } = user;

    [HttpGet("carrinho")]
    public async Task<CarrinhoCliente> ObterCarrinho()
    {
        return null;
    }

    [HttpPost("carrinho")]
    public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
    {
        return CustomResponse();
    }

    [HttpPut("carrinho/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
    {
        return CustomResponse();
    }

    [HttpDelete("carrinho/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        return CustomResponse();
    }
}
