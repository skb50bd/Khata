using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Microsoft.AspNetCore.Http;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public class SalaryIssueService : ISalaryIssueService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IEmployeeService _employees;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public SalaryIssueService(IUnitOfWork db,
            IMapper mapper,
            IEmployeeService employees,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _employees = employees;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<SalaryIssueDto>> Get(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<SalaryIssue, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.Employee.FullName.ToLowerInvariant().Contains(pf.Filter);

            var res = await _db.SalaryIssues.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
            return res.CastList(c => _mapper.Map<SalaryIssueDto>(c));
        }

        public async Task<SalaryIssueDto> Get(int id)
        {
            return _mapper.Map<SalaryIssueDto>(await _db.SalaryIssues.GetById(id));
        }

        public async Task<SalaryIssueDto> Add(SalaryIssueViewModel model)
        {
            var EmployeeVm = _mapper.Map<EmployeeViewModel>(
                await _employees.Get(model.EmployeeId));

            var dm = _mapper.Map<SalaryIssue>(model);

            dm.BalanceBefore = EmployeeVm.Balance;
            dm.Metadata = Metadata.CreatedNew(CurrentUser);

            EmployeeVm.Balance += model.Amount;

            await _employees.Update(EmployeeVm);
            _db.SalaryIssues.Add(dm);
            await _db.CompleteAsync();

            return _mapper.Map<SalaryIssueDto>(dm);
        }

        public async Task<SalaryIssueDto> Update(SalaryIssueViewModel vm)
        {
            var newSalaryIssue = _mapper.Map<SalaryIssue>(vm);
            var originalSalaryIssue = await _db.SalaryIssues.GetById(newSalaryIssue.Id);
            var meta = originalSalaryIssue.Metadata.Modified(CurrentUser);
            originalSalaryIssue.SetValuesFrom(newSalaryIssue);
            originalSalaryIssue.Metadata = meta;

            await _db.CompleteAsync();

            return _mapper.Map<SalaryIssueDto>(originalSalaryIssue);
        }

        public async Task<SalaryIssueDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.SalaryIssues.IsRemoved(id))
                return null;
            await _db.SalaryIssues.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<SalaryIssueDto>(await _db.SalaryIssues.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.SalaryIssues.Exists(id);

        public async Task<SalaryIssueDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<SalaryIssueDto>(await _db.SalaryIssues.GetById(id));
            await _db.SalaryIssues.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<SalaryIssueDto>(dto);
        }
    }
}
