using System.Threading.Tasks;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;
using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface IDebtPaymentService
    {
        Task<DebtPaymentDto> Add(DebtPaymentViewModel model);
        Task<DebtPaymentDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<DebtPaymentDto> Get(int id);
        Task<IPagedList<DebtPaymentDto>> Get(PageFilter pf);
        Task<DebtPaymentDto> Remove(int id);
        Task<DebtPaymentDto> Update(DebtPaymentViewModel vm);
    }
}