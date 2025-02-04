using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebApp.MVC.Extensions;
using DevIO.NerdStore.WebApp.MVC.Models;
using Microsoft.Extensions.Options;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public class AutenticacaoService : Service, IAutenticacaoService
{
    private HttpClient HttpClient { get; }

    public AutenticacaoService(HttpClient httpClient, IOptions<AppSettings> appSetting)
    {
        HttpClient = httpClient;
        HttpClient.BaseAddress = new Uri(appSetting.Value.AutenticacaoUrl);
    }

    public async Task<UsuarioRespostaLogin?> Login(UsuarioLogin usuarioLogin)
    {
        StringContent loginContent = ObterConteudo(usuarioLogin);

        HttpResponseMessage response = await HttpClient.PostAsync("/api/identity/login", loginContent);

        if (!TratarErrosResponse(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }

    public async Task<UsuarioRespostaLogin?> Registro(UsuarioRegistro usuarioRegistro)
    {
        StringContent registroContent = ObterConteudo(usuarioRegistro);

        HttpResponseMessage response = await HttpClient.PostAsync("/api/identity/registrar", registroContent);

        if (!TratarErrosResponse(response))
        {
            return new UsuarioRespostaLogin
            {
                ResponseResult = await DeserializarObjetoResponse<ResponseResult>(response)
            };
        }

        return await DeserializarObjetoResponse<UsuarioRespostaLogin>(response);
    }
}
