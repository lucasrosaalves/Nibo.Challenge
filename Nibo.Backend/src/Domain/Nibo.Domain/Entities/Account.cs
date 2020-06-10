using Nibo.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nibo.Domain.Entities
{
    public class Account : IAggregateRoot
    {
        private List<Transaction> _transactions;

        public Guid Id { get; private set; }
        public string Bank { get; private set; }
        public string Number { get; private set; }
        public IReadOnlyCollection<Transaction> Transactions => _transactions ?? new List<Transaction>();

        protected Account() { }

        public Account(string bank, string number)
        {
            Id = Guid.NewGuid();
            Bank = bank;
            Number = number;
            _transactions = new List<Transaction>();
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
            if (_transactions.IsNullOrEmpty())
            {
                _transactions = new List<Transaction>();
            }


            if(_transactions.Any(p=> transaction.Equals(p)))
            {
                return;
            }

            _transactions.Add(transaction);
        }
    }
}
