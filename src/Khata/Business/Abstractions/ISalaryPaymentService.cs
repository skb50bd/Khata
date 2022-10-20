using System;
using System.Threading.Tasks;
using Brotal;
using Business.PageFilterSort;
using DTOs;
using ViewModels;

namespace Business.Abstractions;

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