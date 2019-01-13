using System.Threading.Tasks;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;
using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface ISupplierPaymentService
    {
        Task<SupplierPaymentDto> Add(SupplierPaymentViewModel model);
        Task<SupplierPaymentDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SupplierPaymentDto> Get(int id);
        Task<IPagedList<SupplierPaymentDto>> Get(PageFilter pf);
        Task<SupplierPaymentDto> Remove(int id);
        Task<SupplierPaymentDto> Update(SupplierPaymentViewModel vm);
    }
}