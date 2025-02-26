using DevIO.NerdStore.Core.DomainObjects;

namespace DevIO.NerdStore.Clientes.API.Models;

public class Endereco(string logradouro, string numero, string complemento, string bairro, string cep, string cidade, string estado, Guid clienteId) : Entity
{
    public string Logradouro { get; private set; } = logradouro;
    public string Numero { get; private set; } = numero;
    public string Complemento { get; private set; } = complemento;
    public string Bairro { get; private set; } = bairro;
    public string Cep { get; private set; } = cep;
    public string Cidade { get; private set; } = cidade;
    public string Estado { get; private set; } = estado;
    public Guid ClienteId { get; private set; } = clienteId;
    public Cliente? Cliente { get; protected set; }
}