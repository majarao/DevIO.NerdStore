namespace DevIO.NerdStore.Core.Data;

public interface IUnitOfWork
{
    Task<bool> Commit();
}
