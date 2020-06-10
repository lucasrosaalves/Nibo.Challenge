using Nibo.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nibo.Domain.Entities
{
    public class Account : IAggregateRoot
    {
        public Guid Id { get; private set; }
        public AccountDetails Details { get; private set; }
        public List<Transaction> Transactions { get; private set; }

        protected Account() { }

        public Account(string bank, string number)
        {
            Id = Guid.NewGuid();
            Details = new AccountDetails(bank, number);
            Transactions = new List<Transaction>();
        }

        public void AddTransactions(IEnumerable<Transaction> transactions)
        {
            if (transactions.IsNullOrEmpty())
            {
                return;
            }

            foreach(var transaction in transactions)
            {
                AddTransaction(transaction);
            }
        }

        public void AddTransaction(Transaction transaction)
        {
            if (Transactions.IsNullOrEmpty())
            {
                Transactions = new List<Transaction>();
            }

            if (Transactions.Any(p=> transaction.Equals(p)))
            {
                return;
            }

            Transactions.Add(transaction);
        }
    }
}
