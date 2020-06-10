using System;

namespace Nibo.Domain.Entities
{
    public class Transaction
    {
        public Transaction(
            string operation, 
            DateTime date,
            decimal value, 
            string description)
        {
            Id = Guid.NewGuid();
            Operation = operation;
            Date = date;
            Value = value;
            Description = description;
        }

        protected Transaction() { }

        public Guid Id { get; private set; }
        public string Operation { get; private set; }
        public DateTime Date { get; private set; }
        public decimal Value { get; private set; }
        public string Description { get; private set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Transaction;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return 
                decimal.Compare(compareTo.Value,Value) == 0  &&
                DateTime.Compare(compareTo.Date, Date) == 0 && 
                compareTo.Description == Description;
        }
    }
}
