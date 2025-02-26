using DevIO.NerdStore.Core.Communication;
using DevIO.NerdStore.WebApp.MVC.Models;

namespace DevIO.NerdStore.WebApp.MVC.Services;

public interface IClienteService
{
    Task<EnderecoViewModel?> ObterEndereco();

    Task<ResponseResult?> AdicionarEndereco(EnderecoViewModel endereco);
}
