using Nibo.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nibo.Domain.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task UpdateOrInsertAsync(Account account);
        Task<IEnumerable<Account>> GetAll();
        Task<Account> GetByBankAccount(string bank, string account);
    }
}
