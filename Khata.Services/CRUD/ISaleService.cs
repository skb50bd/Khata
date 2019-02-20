using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Brotal.Extensions;

namespace Khata.Services.CRUD
{
    public interface ISaleService
    {
        Task<SaleDto> Add(SaleViewModel model);
        Task<SaleDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SaleDto> Get(int id);
        Task<IEnumerable<SaleDto>> GetCustomerSales(int customerId);
        Task<IPagedList<SaleDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<SaleDto> Remove(int id);
        Task<SaleDto> Update(SaleViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}