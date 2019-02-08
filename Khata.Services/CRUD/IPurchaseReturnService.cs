using System;
using System.Threading.Tasks;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;
using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface IPurchaseReturnService
    {
        Task<PurchaseReturnDto> Add(PurchaseReturnViewModel model);
        Task<int> Count(DateTime? from = null, DateTime? to = null);
        Task<PurchaseReturnDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<PurchaseReturnDto> Get(int id);
        Task<IPagedList<PurchaseReturnDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<PurchaseReturnDto> Remove(int id);
        Task<PurchaseReturnDto> Update(PurchaseReturnViewModel vm);
    }
}