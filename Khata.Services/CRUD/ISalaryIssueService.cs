using System;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Brotal.Extensions;

namespace Khata.Services.CRUD
{
    public interface ISalaryIssueService
    {
        Task<SalaryIssueDto> Add(SalaryIssueViewModel model);
        Task<SalaryIssueDto> Delete(int id);
        Task<bool> Exists(int id);
        Task<SalaryIssueDto> Get(int id);
        Task<IPagedList<SalaryIssueDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<SalaryIssueDto> Remove(int id);
        Task<SalaryIssueDto> Update(SalaryIssueViewModel vm);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}