using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Extensions;

public class SummaryViewComponent : ViewComponent
{
    public IViewComponentResult Invoke() => View();
}
