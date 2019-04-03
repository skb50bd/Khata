using System;
using System.Threading.Tasks;
using Brotal;
using Brotal.Extensions;
using Business.PageFilterSort;
using DTOs;
using ViewModels;

namespace Business.Abstractions
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