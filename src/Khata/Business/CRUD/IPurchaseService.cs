using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using DTOs;
using Business.PageFilterSort;
using ViewModels;

using Brotal.Extensions;

namespace Business.CRUD
{
    public interface IPurchaseService
    {
        Task<PurchaseDto> Add(PurchaseViewModel model);
        Task<PurchaseDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<PurchaseDto> Get(int id);
        Task<IPagedList<PurchaseDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<PurchaseDto> Remove(int id);
        Task<PurchaseDto> Update(PurchaseViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
        Task<IEnumerable<PurchaseDto>> GetSupplierPurchases(int supplierId);
    }
}