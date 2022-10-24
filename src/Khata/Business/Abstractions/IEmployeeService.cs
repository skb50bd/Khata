using System;
using System.Threading.Tasks;
using Business.PageFilterSort;
using Domain;
using DTOs;
using ViewModels;

namespace Business.Abstractions;

public interface IEmployeeService
{
    Task<EmployeeDto> Add(EmployeeViewModel model);
    Task<EmployeeDto> Delete(int id);
    Task<bool> Exists(int id);
    Task<EmployeeDto> Get(int id);
    Task<IPagedList<EmployeeDto>> Get(PageFilter pf, DateTime? from = null, DateTime? to = null);
    Task<EmployeeDto> Remove(int id);
    Task<EmployeeDto> Update(EmployeeViewModel vm);
    Task<int> Count(DateTime? from, DateTime? to);
}