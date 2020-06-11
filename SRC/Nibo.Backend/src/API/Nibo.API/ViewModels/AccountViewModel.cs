using Nibo.Util.Extensions;
using System.Collections.Generic;

namespace Nibo.API.ViewModels
{
    public class AccountViewModel
    {
        public AccountViewModel(
            string bank, 
            string accountNumber, 
            decimal balance,
            IEnumerable<TransactionViewModel> transactions)
        {
            Bank = bank;
            AccountNumber = accountNumber;
            Balance = balance.ToMoneyStringFormat();
            Transactions = transactions ?? new List<TransactionViewModel>();
        }

        public string Bank { get; }
        public string AccountNumber { get; }
        public string Balance { get; }
        public IEnumerable<TransactionViewModel> Transactions { get; }
    }
}
