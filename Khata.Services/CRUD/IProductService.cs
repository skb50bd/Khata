using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface IProductService
    {
        Task<IPagedList<ProductDto>> Get(PageFilter pf);
        Task<ProductDto> Get(int id);
        Task<ProductDto> Add(ProductViewModel model);
        Task<ProductDto> Update(ProductViewModel vm);
        Task<ProductDto> Remove(int id);
        Task<bool> Exists(int id);
        Task<ProductDto> Delete(int id);
    }
}