using System;

namespace Nibo.API.ViewModels
{
    public class TransactionViewModel
    {
        public TransactionViewModel(
            DateTime date, 
            decimal value, 
            string description)
        {
            Date = date;
            Value = value;
            Description = description;
        }

        public DateTime Date { get;}
        public decimal Value { get;}
        public string Description { get; }
    }

}
