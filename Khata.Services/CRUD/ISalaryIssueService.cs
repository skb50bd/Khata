using System.Threading.Tasks;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;
using SharedLibrary;

namespace Khata.Services.CRUD
{
    public interface ISalaryIssueService
    {
        Task<SalaryIssueDto> Add(SalaryIssueViewModel model);
        Task<SalaryIssueDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SalaryIssueDto> Get(int id);
        Task<IPagedList<SalaryIssueDto>> Get(PageFilter pf);
        Task<SalaryIssueDto> Remove(int id);
        Task<SalaryIssueDto> Update(SalaryIssueViewModel vm);
    }
}