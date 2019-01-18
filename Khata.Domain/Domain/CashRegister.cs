using System.Collections.Generic;

namespace Khata.Domain
{
    public class CashRegister : Entity
    {
        public decimal Balance { get; private set; } = 0;
        public Metadata Metadata { get; set; }

        public ICollection<Withdrawal> Withdrawals { get; set; } = new List<Withdrawal>();
        public ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

        public void AddTransaction(Withdrawal item)
        {
            Withdrawals.Add(item);
            Balance -= item.Amount;
        }

        public void AddTransaction(Deposit item)
        {
            Deposits.Add(item);
            Balance += item.Amount;
        }
    }
}
