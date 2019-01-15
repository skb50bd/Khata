using System.Threading.Tasks;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;
using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface IEmployeeService
    {
        Task<EmployeeDto> Add(EmployeeViewModel model);
        Task<EmployeeDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<EmployeeDto> Get(int id);
        Task<IPagedList<EmployeeDto>> Get(PageFilter pf);
        Task<EmployeeDto> Remove(int id);
        Task<EmployeeDto> Update(EmployeeViewModel vm);
    }
}