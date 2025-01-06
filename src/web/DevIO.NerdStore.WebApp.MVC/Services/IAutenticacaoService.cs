using DevIO.NerdStore.WebApp.MVC.Models;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public interface IAutenticacaoService
{
    Task<UsuarioRespostaLogin?> Login(UsuarioLogin usuarioLogin);

    Task<UsuarioRespostaLogin?> Registro(UsuarioRegistro usuarioRegistro);
}
