using Nibo.Domain.Entities;

namespace Nibo.Domain.Repositories
{
    public interface IRepository<T>  where T : IAggregateRoot
    {
    }
}
