using DevIO.NerdStore.WebApp.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.NerdStore.WebApp.MVC.Controllers;

public class MainController : Controller
{
    protected bool ResponsePossuiErros(ResponseResult? resposta)
    {
        if (resposta is not null && resposta?.Errors?.Mensagens.Count > 0)
        {
            foreach (string mensagem in resposta.Errors.Mensagens)
                ModelState.AddModelError(string.Empty, mensagem);

            return true;
        }

        return false;
    }
}
