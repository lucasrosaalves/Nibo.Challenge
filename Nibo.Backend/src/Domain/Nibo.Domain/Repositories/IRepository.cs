using Nibo.Domain.Entities;
using System;

namespace Nibo.Domain.Repositories
{
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
