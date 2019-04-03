using System;
using System.Threading.Tasks;
using Brotal;
using Brotal.Extensions;
using Business.PageFilterSort;
using DTOs;
using ViewModels;

namespace Business.Abstractions
{
    public interface IServiceService
    {
        Task<IPagedList<ServiceDto>> Get(int outletId, PageFilter pf, DateTime? from = null, DateTime? to = null);
        Task<ServiceDto> Get(int id);
        Task<ServiceDto> Add(ServiceViewModel model);
        Task<ServiceDto> Update(ServiceViewModel vm);
        Task<ServiceDto> Remove(int id);
        Task<bool> Exists(int id);
        Task<ServiceDto> Delete(int id);
        Task<int> Count(DateTime? from, DateTime? to);
    }
}