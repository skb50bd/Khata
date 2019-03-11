using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DTOs;
using Business.PageFilterSort;
using ViewModels;

using Brotal.Extensions;

namespace Business.CRUD
{
    public interface IDebtPaymentService
    {
        Task<DebtPaymentDto> Add(DebtPaymentViewModel model);
        Task<DebtPaymentDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<DebtPaymentDto> Get(int id);
        Task<IEnumerable<DebtPaymentDto>> GetCustomerDebtPayments(int customerId);
        Task<IPagedList<DebtPaymentDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<DebtPaymentDto> Remove(int id);
        Task<DebtPaymentDto> Update(DebtPaymentViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}