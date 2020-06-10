using System.Threading.Tasks;

namespace Nibo.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
