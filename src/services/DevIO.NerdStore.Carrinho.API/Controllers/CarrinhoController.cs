using DevIO.NerdStore.Carrinho.API.Data;
using DevIO.NerdStore.Carrinho.API.Model;
using DevIO.NerdStore.WebAPI.Core.Controllers;
using DevIO.NerdStore.WebAPI.Core.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Carrinho.API.Controllers;

[Authorize]
public class CarrinhoController(IAspNetUser user, CarrinhoContext context) : MainController
{
    private IAspNetUser AspNetUser { get; } = user;
    private CarrinhoContext Context { get; } = context;

    [HttpGet("carrinho")]
    public async Task<CarrinhoCliente?> ObterCarrinho() => await ObterCarrinhoCliente();

    [HttpPost("carrinho")]
    public async Task<IActionResult> AdicionarItemCarrinho(CarrinhoItem item)
    {
        CarrinhoCliente? carrinho = await ObterCarrinhoCliente();

        if (carrinho is null)
            ManipularNovoCarrinho(item);
        else
            ManipularCarrinhoExistente(carrinho, item);

        if (!OperacaoValida())
            return CustomResponse();

        await PersistirDados();

        return CustomResponse();
    }

    [HttpPut("carrinho/{produtoId}")]
    public async Task<IActionResult> AtualizarItemCarrinho(Guid produtoId, CarrinhoItem item)
    {
        CarrinhoCliente? carrinho = await ObterCarrinhoCliente();
        CarrinhoItem? itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho, item);

        if (itemCarrinho == null)
            return CustomResponse();

        carrinho!.AtualizarUnidades(itemCarrinho, item.Quantidade);

        ValidarCarrinho(carrinho);

        if (!OperacaoValida())
            return CustomResponse();

        Context.CarrinhoItens.Update(itemCarrinho);
        Context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();

        return CustomResponse();
    }

    [HttpDelete("carrinho/{produtoId}")]
    public async Task<IActionResult> RemoverItemCarrinho(Guid produtoId)
    {
        CarrinhoCliente? carrinho = await ObterCarrinhoCliente();
        CarrinhoItem? itemCarrinho = await ObterItemCarrinhoValidado(produtoId, carrinho);

        if (itemCarrinho is null)
            return CustomResponse();

        ValidarCarrinho(carrinho!);

        if (!OperacaoValida())
            return CustomResponse();

        carrinho!.RemoverItem(itemCarrinho);

        Context.CarrinhoItens.Remove(itemCarrinho);
        Context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();

        return CustomResponse();
    }

    [HttpPost]
    [Route("carrinho/aplicar-voucher")]
    public async Task<IActionResult> AplicarVoucher(Voucher voucher)
    {
        CarrinhoCliente? carrinho = await ObterCarrinhoCliente();

        if (carrinho is null)
            return CustomResponse();

        carrinho.AplicarVoucher(voucher);

        Context.CarrinhoCliente.Update(carrinho);

        await PersistirDados();

        return CustomResponse();
    }

    private async Task<CarrinhoCliente?> ObterCarrinhoCliente() =>
        await Context.CarrinhoCliente
            .Include(c => c.Itens)
            .FirstOrDefaultAsync(c => c.ClienteId == AspNetUser.ObterUserId());

    private void ManipularNovoCarrinho(CarrinhoItem item)
    {
        CarrinhoCliente carrinho = new(AspNetUser.ObterUserId());

        carrinho.AdicionarItem(item);

        ValidarCarrinho(carrinho);

        Context.CarrinhoCliente.Add(carrinho);
    }

    private void ManipularCarrinhoExistente(CarrinhoCliente carrinho, CarrinhoItem item)
    {
        bool produtoItemExistente = carrinho.CarrinhoItemExistente(item);

        carrinho.AdicionarItem(item);

        ValidarCarrinho(carrinho);

        if (produtoItemExistente)
            Context.CarrinhoItens.Update(carrinho.ObterPorProdutoId(item.ProdutoId)!);
        else
            Context.CarrinhoItens.Add(item);

        Context.CarrinhoCliente.Update(carrinho);
    }

    private async Task<CarrinhoItem?> ObterItemCarrinhoValidado(Guid produtoId, CarrinhoCliente? carrinho, CarrinhoItem? item = null)
    {
        if (item is not null && produtoId != item.ProdutoId)
        {
            AdicionarErroProcessamento("O item não corresponde ao informado");
            return null;
        }

        if (carrinho is null)
        {
            AdicionarErroProcessamento("Carrinho não encontrado");
            return null;
        }

        CarrinhoItem? itemCarrinho = await Context.CarrinhoItens
            .FirstOrDefaultAsync(i => i.CarrinhoId == carrinho.Id && i.ProdutoId == produtoId);

        if (itemCarrinho is null || !carrinho.CarrinhoItemExistente(itemCarrinho))
        {
            AdicionarErroProcessamento("O item não está no carrinho");
            return null;
        }

        return itemCarrinho;
    }

    private bool ValidarCarrinho(CarrinhoCliente carrinho)
    {
        if (carrinho.EhValido())
            return true;

        carrinho.ValidationResult?.Errors.ToList().ForEach(e => AdicionarErroProcessamento(e.ErrorMessage));
        return false;
    }

    private async Task PersistirDados()
    {
        int result = await Context.SaveChangesAsync();

        if (result <= 0)
            AdicionarErroProcessamento("Não foi possível persistir os dados no banco");
    }
}
