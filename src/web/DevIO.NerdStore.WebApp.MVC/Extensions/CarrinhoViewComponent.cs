using DevIO.NerdStore.WebApp.MVC.Models;
using DevIO.NerdStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Extensions;

public class CarrinhoViewComponent(ICarrinhoService carrinhoService) : ViewComponent
{
    private ICarrinhoService CarrinhoService { get; } = carrinhoService;

    public async Task<IViewComponentResult> InvokeAsync() => View(await CarrinhoService.ObterCarrinho() ?? new CarrinhoViewModel());
}
