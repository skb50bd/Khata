using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Brotal;
using Brotal.Extensions;
using Business.Abstractions;
using Business.PageFilterSort;
using Data.Core;
using Domain;
using DTOs;
using Microsoft.AspNetCore.Http;
using ViewModels;
using Metadata = Domain.Metadata;

namespace Business.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

    public EmployeeService(IUnitOfWork db, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IPagedList<EmployeeDto>> Get(
        PageFilter pf,
        DateTime? from = null,
        DateTime? to = null)
    {
        var predicate = string.IsNullOrEmpty(pf.Filter)
            ? (Expression<Func<Employee, bool>>)(p => true)
            : p => p.Id.ToString() == pf.Filter
                   || p.FullName.ToLowerInvariant().Contains(pf.Filter)
                   || p.Designation.ToLowerInvariant().Contains(pf.Filter)
                   || p.NIdNumber.ToLowerInvariant().Contains(pf.Filter)
                   || p.Phone.Contains(pf.Filter)
                   || p.Email.Contains(pf.Filter);

        var res = await _db.Employees.Get(
            predicate,
            p => p.Id,
            pf.PageIndex,
            pf.PageSize,
            from, to
        );
        return res.CastList(c => _mapper.Map<EmployeeDto>(c));
    }

    public async Task<EmployeeDto> Get(int id)
    {
        return _mapper.Map<EmployeeDto>(await _db.Employees.GetById(id));
    }

    public async Task<EmployeeDto> Add(EmployeeViewModel model)
    {
        var dm = _mapper.Map<Employee>(model);
        dm.Metadata = Metadata.CreatedNew(CurrentUser);
        _db.Employees.Add(dm);
        await _db.CompleteAsync();

        return _mapper.Map<EmployeeDto>(dm);
    }

    public async Task<EmployeeDto> Update(EmployeeViewModel vm)
    {
        var newEmployee = _mapper.Map<Employee>(vm);
        var originalEmployee = await _db.Employees.GetById(newEmployee.Id);
        var meta = originalEmployee.Metadata.ModifiedBy(CurrentUser);
        originalEmployee.SetValuesFrom(newEmployee);
        originalEmployee.Metadata = meta;

        await _db.CompleteAsync();

        return _mapper.Map<EmployeeDto>(originalEmployee);
    }

    public async Task<EmployeeDto> Remove(int id)
    {
        if (!(await Exists(id))
            || await _db.Employees.IsRemoved(id))
            return null;
        await _db.Employees.Remove(id);
        await _db.CompleteAsync();
        return _mapper.Map<EmployeeDto>(await _db.Employees.GetById(id));
    }

    public async Task<bool> Exists(int id) => await _db.Employees.Exists(id);

    public async Task<EmployeeDto> Delete(int id)
    {
        if (!(await Exists(id)))
            return null;

        var dto = _mapper.Map<EmployeeDto>(await _db.Employees.GetById(id));
        await _db.Employees.Delete(id);
        await _db.CompleteAsync();
        return _mapper.Map<EmployeeDto>(dto);
    }

    public async Task<int> Count(DateTime? from = null, DateTime? to = null)
    {
        return await _db.Employees.Count(from, to);
    }
}