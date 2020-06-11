using Nibo.Util.Extensions;
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
            Date = date.ToString("dd/MM/yyyy HH:mm");
            Value = value.ToMoneyStringFormat();
            Description = description;
        }

        public string Date { get;}
        public string Value { get;}
        public string Description { get; }
    }

}
