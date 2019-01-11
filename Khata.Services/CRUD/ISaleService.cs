using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface ISaleService
    {
        Task<SaleDto> Add(SaleViewModel model);
        Task<SaleDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SaleDto> Get(int id);
        Task<IPagedList<SaleDto>> Get(PageFilter pf);
        Task<SaleDto> Remove(int id);
        Task<SaleDto> Update(SaleViewModel vm);
    }
}