using DevIO.NerdStore.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Extensions;

public class PaginacaoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPagedList modeloPaginado) => View(modeloPaginado);
}