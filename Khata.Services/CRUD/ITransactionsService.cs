using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Domain;
using Khata.Services.PageFilterSort;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface ITransactionsService
    {
        Task<Deposit> Add(Deposit model);
        Task<Withdrawal> Add(Withdrawal model);
        Task<Deposit> DeleteDeposit(int id);
        Task<Withdrawal> DeleteWithdrawal(int id);
        Task<bool> DepositExists(int id);
        Task<IEnumerable<Deposit>> GetDeposits(DateTimeOffset from, DateTimeOffset to);
        Task<IPagedList<Deposit>> GetDeposits(PageFilter pf);
        Task<IEnumerable<Withdrawal>> GetWithdrawals(DateTimeOffset from, DateTimeOffset to);
        Task<IPagedList<Withdrawal>> GetWithdrawals(PageFilter pf);

        Task<Deposit> GetDepositById(int id);
        Task<Withdrawal> GetWithdrawalById(int id);
        Task<bool> WithdrawalExists(int id);
    }
}