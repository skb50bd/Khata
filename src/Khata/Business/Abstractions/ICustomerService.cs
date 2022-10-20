using System;
using System.Threading.Tasks;
using Brotal;
using Business.PageFilterSort;
using DTOs;
using ViewModels;

namespace Business.Abstractions;

public interface ICustomerService
{
    Task<CustomerDto> Add(CustomerViewModel model);
    Task<IPagedList<CustomerDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
    Task<CustomerDto> Get(int id);
    Task<CustomerDto> Update(CustomerViewModel vm);
    Task<CustomerDto> Remove(int id);
    Task<bool> Exists(int id);
    Task<CustomerDto> Delete(int id);
    Task<int> Count(DateTime? from, DateTime? to);
}