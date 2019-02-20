using System;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Brotal.Extensions;

namespace Khata.Services.CRUD
{
    public interface ISupplierService
    {
        Task<SupplierDto> Add(SupplierViewModel model);
        Task<SupplierDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SupplierDto> Get(int id);
        Task<IPagedList<SupplierDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<SupplierDto> Remove(int id);
        Task<SupplierDto> Update(SupplierViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}