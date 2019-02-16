
using System.Linq;

using Khata.Data.Core;
using Khata.Domain;

namespace Khata.Data.Persistence
{
    public class DepositRepository : Repository<Deposit>, IRepository<Deposit>
    {
        public DepositRepository(KhataContext context) : base(context) { }
        public override void Add(Deposit item)
        {
            Context.Deposits.Add(item);
            Context.CashRegister.FirstOrDefault().Balance += item.Amount;
        }
    }

    public class WithdrawalRepository : Repository<Withdrawal>, IRepository<Withdrawal>
    {
        public WithdrawalRepository(KhataContext context) : base(context) { }
        public override void Add(Withdrawal item)
        {
            Context.Withdrawals.Add(item);
            Context.CashRegister.FirstOrDefault().Balance -= item.Amount;
        }
    }
}
