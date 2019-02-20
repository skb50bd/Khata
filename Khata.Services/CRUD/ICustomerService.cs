using System;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Brotal.Extensions;

namespace Khata.Services.CRUD
{
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
}