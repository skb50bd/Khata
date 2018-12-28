using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface IServiceService
    {
        Task<IPagedList<ServiceDto>> Get(PageFilter pf);
        Task<ServiceDto> Get(int id);
        Task<ServiceDto> Add(ServiceViewModel model);
        Task<ServiceDto> Update(ServiceViewModel vm);
        Task<ServiceDto> Remove(int id);
        Task<bool> Exists(int id);
        Task<ServiceDto> Delete(int id);
    }
}