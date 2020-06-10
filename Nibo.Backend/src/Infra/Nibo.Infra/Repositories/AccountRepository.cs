using MongoDB.Driver;
using Nibo.Domain.Entities;
using Nibo.Domain.Repositories;
using Nibo.Domain.Settings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nibo.Infra.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IMongoCollection<Account> _account;

        public AccountRepository(IDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _account = database.GetCollection<Account>(settings.CollectionName);
        }

        public async Task UpdateOrInsertAsync(Account account)
        {
            var register = await GetByBankAccount(account.Details.Bank, account.Details.AccountNumber);
            if(register is null)
            {
                await _account.InsertOneAsync(account);
                return;
            }

            await _account.ReplaceOneAsync(p => p.Id == account.Id, account);
        }

        public async Task<IEnumerable<Account>> GetAll()
        {
            return await _account.Find(p => true).ToListAsync();
        }

        public Task<Account> GetByBankAccount(string bank, string account)
        {
            return _account.Find<Account>(p => p.Details.Bank == bank && p.Details.AccountNumber == account).FirstOrDefaultAsync();
        }

    }
}
