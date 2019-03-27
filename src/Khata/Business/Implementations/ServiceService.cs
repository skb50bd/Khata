using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Brotal.Extensions;
using Business.Abstractions;
using Business.PageFilterSort;
using Data.Core;
using Domain;
using DTOs;
using Microsoft.AspNetCore.Http;
using ViewModels;

namespace Business.Implementations
{
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => 
            _httpContextAccessor.HttpContext.User.Identity.Name;

        public ServiceService(
            IUnitOfWork db, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<ServiceDto>> Get(
            int outletId,
            PageFilter pf,
            DateTime? from = null,
            DateTime? to = null)
        {
            int id;
            Expression<Func<Service, bool>> fuzzySearch =
                p => p.Id.ToString() == pf.Filter
                    || p.Name.ToLowerInvariant().Contains(pf.Filter);
            Expression<Func<Service, bool>> strictSearch =
                p => (int.TryParse(pf.Filter, out id) && p.Id == id)
                    || p.Name.ToLowerInvariant().StartsWith(pf.Filter);

            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (p => !p.IsRemoved)
                : strictSearch;

            if(outletId != 0)
            {
                predicate = predicate.And(s => s.OutletId == outletId);
            }

            var res = await _db.Services.Get(
                predicate,
                s => s.Id,
                pf.PageIndex,
                pf.PageSize
            );
            return res.CastList(c => _mapper.Map<ServiceDto>(c));
        }

        public async Task<ServiceDto> Get(int id)
        {
            return _mapper.Map<ServiceDto>(await _db.Services.GetById(id));
        }

        public async Task<ServiceDto> Add(ServiceViewModel model)
        {
            var dm = _mapper.Map<Service>(model);
            dm.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Services.Add(dm);
            await _db.CompleteAsync();

            return _mapper.Map<ServiceDto>(dm);
        }

        public async Task<ServiceDto> Update(ServiceViewModel vm)
        {
            var newService = _mapper.Map<Service>(vm);
            newService.Outlet = await _db.Outlets.GetById(vm.OutletId);
            var originalService = await _db.Services.GetById(newService.Id);
            var meta = originalService.Metadata.Modified(CurrentUser);
            originalService.SetValuesFrom(newService);
            originalService.Metadata = meta;

            await _db.CompleteAsync();

            return _mapper.Map<ServiceDto>(originalService);
        }

        public async Task<ServiceDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.Services.IsRemoved(id))
                return null;
            await _db.Services.Remove(id);
            await _db.CompleteAsync();

            var dto = _mapper.Map<ServiceDto>(await _db.Services.GetById(id));
            dto.Outlet = null;
            return dto;
        }

        public async Task<bool> Exists(int id) => await _db.Services.Exists(id);

        public async Task<ServiceDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<ServiceDto>(await _db.Services.GetById(id));
            await _db.Services.Delete(id);
            await _db.CompleteAsync();
            dto.Outlet = null;
            return dto;
        }

        public async Task<int> Count(DateTime? from = null, DateTime? to = null)
        {
            return await _db.Services.Count(from, to);
        }
    }
}
