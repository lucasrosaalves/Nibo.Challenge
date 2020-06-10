using Nibo.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nibo.Domain.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task AddAsync();
        Task UpdateAsync();
        Task<Account> GetByBankAccount(string bank, string account);
        Task<IEnumerable<Account>> GetAll();
    }
}
