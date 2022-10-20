using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Brotal;
using Business.PageFilterSort;
using Domain;
using ViewModels;

namespace Business.Abstractions;

public interface ITransactionsService
{
    Task<Deposit> Add(DepositViewModel model);
    Task<Withdrawal> Add(WithdrawalViewModel model);
    Task<Deposit> DeleteDeposit(int id);
    Task<Withdrawal> DeleteWithdrawal(int id);
    Task<bool> DepositExists(int id);
    Task<IEnumerable<Deposit>> GetDeposits(DateTime from, DateTime to);
    Task<IPagedList<Deposit>> GetDeposits(PageFilter pf);
    Task<IEnumerable<Withdrawal>> GetWithdrawals(DateTime from, DateTime to);
    Task<IPagedList<Withdrawal>> GetWithdrawals(PageFilter pf);

    Task<Deposit> GetDepositById(int id);
    Task<Withdrawal> GetWithdrawalById(int id);
    Task<bool> WithdrawalExists(int id);
}