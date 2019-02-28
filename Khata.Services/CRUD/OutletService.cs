using AutoMapper;
using Brotal.Extensions;
using Khata.Data.Core;
using Khata.Domain;
using Khata.DTOs;
using Khata.ViewModels;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Khata.Services.CRUD
{
    public class OutletService : IOutletService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;
        public OutletService(
            IUnitOfWork db,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<OutletDto> Add(OutletViewModel model) {
            var dm = _mapper.Map<Outlet>(model);
            dm.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Outlets.Add(dm);
            await _db.CompleteAsync();

            return _mapper.Map<OutletDto>(dm);
        }

        public async Task<int> Count() => await _db.Outlets.Count();
        public async Task<OutletDto> Delete(int id) {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<OutletDto>(await _db.Outlets.GetById(id));
            await _db.Outlets.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<OutletDto>(dto);
        }
        public async Task<bool> Exists(int id) => await _db.Outlets.Exists(id);
        public async Task<IEnumerable<OutletDto>> Get() {
            var dms = await _db.Outlets.GetAll();
            var dtos = new List<OutletDto>();
            foreach(var o in dms)
            {
                dtos.Add(_mapper.Map<OutletDto>(o));
            }
            return dtos;
        }
        public async Task<OutletDto> Get(int id) 
            => _mapper.Map<OutletDto>(await _db.Outlets.GetById(id));
        public async Task<OutletDto> Remove(int id) {
            if (!(await Exists(id))
                 || await _db.Outlets.IsRemoved(id))
                return null;
            await _db.Outlets.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<OutletDto>(
                await _db.Outlets.GetById(id));
        }

        public async Task<OutletDto> Update(OutletViewModel vm) {
            var newDm = _mapper.Map<Outlet>(vm);
            var originalDm = await _db.Outlets.GetById(newDm.Id);
            var meta = originalDm.Metadata.Modified(CurrentUser);
            originalDm.SetValuesFrom(newDm);
            originalDm.Metadata = meta;

            await _db.CompleteAsync();

            return _mapper.Map<OutletDto>(originalDm);
        }
    }
}
