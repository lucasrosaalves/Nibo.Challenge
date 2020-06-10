using Nibo.Domain.Entities;
using Nibo.Domain.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nibo.Infra.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        public IUnitOfWork UnitOfWork => throw new System.NotImplementedException();

        public Task AddAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Task<IEnumerable<Account>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task<Account> GetByBankAccount(string bank, string account)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}
