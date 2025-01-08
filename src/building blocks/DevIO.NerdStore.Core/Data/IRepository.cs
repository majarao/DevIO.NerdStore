using DevIO.NerdStore.Core.DomainObjects;

namespace DevIO.NerdStore.Core.Data;

public interface IRepository<T> : IDisposable where T : IAggregateRoot
{

}
