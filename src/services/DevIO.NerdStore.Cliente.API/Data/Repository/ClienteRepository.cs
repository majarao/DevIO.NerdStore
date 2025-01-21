using DevIO.NerdStore.Clientes.API.Models;
using DevIO.NerdStore.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Clientes.API.Data.Repository;

public class ClienteRepository(ClientesContext context) : IClienteRepository
{
    private ClientesContext Context { get; } = context;

    public IUnitOfWork UnitOfWork => Context;

    public async Task<IEnumerable<Cliente>> ObterTodos() => await Context.Clientes.AsNoTracking().ToListAsync();

    public Task<Cliente?> ObterPorCpf(string cpf) => Context.Clientes.FirstOrDefaultAsync(c => c.Cpf!.Numero == cpf);

    public void Adicionar(Cliente cliente) => Context.Clientes.Add(cliente);

    public void Dispose() => Context.Dispose();
}
