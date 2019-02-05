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
    public class ServiceService : IServiceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public ServiceService(IUnitOfWork db, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<ServiceDto>> Get(
            PageFilter pf,
            DateTime? from = null,
            DateTime? to = null)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<Service, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.Name.ToLowerInvariant().Contains(pf.Filter);

            var res = await _db.Services.Get(
                predicate,
                p => p.Id,
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
            return _mapper.Map<ServiceDto>(await _db.Services.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.Services.Exists(id);

        public async Task<ServiceDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<ServiceDto>(await _db.Services.GetById(id));
            await _db.Services.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<ServiceDto>(dto);
        }

        public async Task<int> Count(DateTime? from = null, DateTime? to = null)
        {
            return await _db.Services.Count(from, to);
        }
    }
}
