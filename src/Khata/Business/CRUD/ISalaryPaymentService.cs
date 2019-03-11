using System;
using System.Threading.Tasks;

using DTOs;
using Business.PageFilterSort;
using ViewModels;

using Brotal.Extensions;

namespace Business.CRUD
{
    public interface ISalaryPaymentService
    {
        Task<SalaryPaymentDto> Add(SalaryPaymentViewModel model);
        Task<SalaryPaymentDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SalaryPaymentDto> Get(int id);
        Task<IPagedList<SalaryPaymentDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<SalaryPaymentDto> Remove(int id);
        Task<SalaryPaymentDto> Update(SalaryPaymentViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}