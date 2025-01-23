namespace DevIO.NerdStore.Core.Messages.Integration;

public class UsuarioRegistradoIntegrationEvent(Guid id, string? nome, string? email, string? cpf) : IntegrationEvent
{
    public Guid Id { get; private set; } = id;
    public string? Nome { get; private set; } = nome;
    public string? Email { get; private set; } = email;
    public string? Cpf { get; private set; } = cpf;
}
