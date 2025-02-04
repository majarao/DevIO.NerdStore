using DevIO.NerdStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Extensions;

public class CarrinhoViewComponent(IComprasBFFService bffService) : ViewComponent
{
    private IComprasBFFService BFFService { get; } = bffService;

    public async Task<IViewComponentResult> InvokeAsync() => View(await BFFService.ObterQuantidadeCarrinho());
}
