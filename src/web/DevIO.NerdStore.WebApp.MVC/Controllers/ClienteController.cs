using DevIO.NerdStore.WebApp.MVC.Models;
using DevIO.NerdStore.WebApp.MVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Controllers;

[Authorize]
public class ClienteController(IClienteService clienteService) : MainController
{
    private IClienteService Service { get; } = clienteService;

    [HttpPost]
    public async Task<IActionResult> NovoEndereco(EnderecoViewModel endereco)
    {
        Core.Communication.ResponseResult? response = await Service.AdicionarEndereco(endereco);

        if (ResponsePossuiErros(response)) TempData["Erros"] =
            ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList();

        return RedirectToAction("EnderecoEntrega", "Pedido");
    }
}
