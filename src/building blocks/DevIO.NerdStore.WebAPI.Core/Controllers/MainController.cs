using DevIO.NerdStore.Core.Communication;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DevIO.NerdStore.WebAPI.Core.Controllers;

[ApiController]
public abstract class MainController : Controller
{
    protected ICollection<string> Erros = [];

    protected ActionResult CustomResponse(object? result = null)
    {
        if (OperacaoValida())
            return Ok(result);

        return BadRequest(new ValidationProblemDetails(new Dictionary<string, string[]>
        {
            {
                "Mensagens",
                Erros.ToArray()
            }
        }));
    }

    protected ActionResult CustomResponse(ModelStateDictionary modelState)
    {
        IEnumerable<ModelError> erros = modelState.Values.SelectMany(e => e.Errors);
        foreach (ModelError erro in erros)
            AdicionarErroProcessamento(erro.ErrorMessage);

        return CustomResponse();
    }

    protected ActionResult CustomResponse(ValidationResult validationResult)
    {
        foreach (ValidationFailure? erro in validationResult.Errors)
            AdicionarErroProcessamento(erro.ErrorMessage);

        return CustomResponse();
    }

    protected bool OperacaoValida() => Erros.Count == 0;

    protected void AdicionarErroProcessamento(string erro) => Erros.Add(erro);

    protected void LimparErroProcessamento() => Erros.Clear();

    protected bool ResponsePossuiErros(ResponseResult? resposta)
    {
        if (resposta is null || resposta.Errors.Mensagens.Count == 0)
            return false;

        foreach (string mensagem in resposta.Errors.Mensagens)
            AdicionarErroProcessamento(mensagem);

        return true;
    }

    protected ActionResult CustomResponse(ResponseResult? resposta)
    {
        ResponsePossuiErros(resposta);

        return CustomResponse();
    }

}
