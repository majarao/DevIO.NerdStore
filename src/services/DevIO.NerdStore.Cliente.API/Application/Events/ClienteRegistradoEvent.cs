using DevIO.NerdStore.Core.Messages;

namespace DevIO.NerdStore.Clientes.API.Application.Events;

public class ClienteRegistradoEvent(Guid id, string? nome, string? email, string? cpf) : Event
{
    public Guid Id { get; private set; } = id;
    public string? Nome { get; private set; } = nome;
    public string? Email { get; private set; } = email;
    public string? Cpf { get; private set; } = cpf;
}
