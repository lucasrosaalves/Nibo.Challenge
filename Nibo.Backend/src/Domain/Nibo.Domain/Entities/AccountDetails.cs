namespace Nibo.Domain.Entities
{
    public class AccountDetails
    {
        public AccountDetails(string bank, string accountNumber)
        {
            Bank = bank;
            AccountNumber = accountNumber;
        }

        protected AccountDetails() { }

        public string Bank { get; private set; }
        public string AccountNumber { get; private set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as AccountDetails;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return compareTo.Bank == Bank && compareTo.AccountNumber == AccountNumber;
        }

        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + (Bank + AccountNumber).GetHashCode();
        }

    }
}
