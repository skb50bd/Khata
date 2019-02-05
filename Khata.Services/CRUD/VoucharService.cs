using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.DTOs;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Http;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public class VoucharService : IVoucharService
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public VoucharService(
            IUnitOfWork db,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<VoucharDto>> Get(PageFilter pf,
            DateTime? from = null,
            DateTime? to = null)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<Vouchar, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.Supplier.FullName.ToLowerInvariant().Contains(pf.Filter)
                    || p.Supplier.CompanyName.ToLowerInvariant().Contains(pf.Filter)
                    || p.Supplier.Phone.Contains(pf.Filter)
                    || p.Supplier.Email.Contains(pf.Filter);

            var res = await _db.Vouchars.Get(
                predicate,
                p => p.Id,
                pf.PageIndex,
                pf.PageSize,
                from, to
            );
            return res.CastList(c => _mapper.Map<VoucharDto>(c));
        }

        public async Task<VoucharDto> Get(int id)
        {
            return _mapper.Map<VoucharDto>(await _db.Vouchars.GetById(id));
        }

        public async Task<VoucharDto> Add(Vouchar model)
        {
            model.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Vouchars.Add(model);
            await _db.CompleteAsync();

            return _mapper.Map<VoucharDto>(model);
        }

        public async Task<VoucharDto> SetPurchase(int voucharId, int saleId)
        {
            var vouchar = await _db.Vouchars.GetById(voucharId);
            vouchar.PurchaseId = saleId;
            await _db.CompleteAsync();

            return _mapper.Map<VoucharDto>(vouchar);
        }

        public async Task<VoucharDto> SetSupplierPayment(int voucharId, int debtPaymentId)
        {
            var vouchar = await _db.Vouchars.GetById(voucharId);
            vouchar.SupplierPaymentId = debtPaymentId;
            await _db.CompleteAsync();

            return _mapper.Map<VoucharDto>(vouchar);
        }

        public async Task<VoucharDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.Vouchars.IsRemoved(id))
                return null;
            await _db.Vouchars.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<VoucharDto>(await _db.Vouchars.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.Vouchars.Exists(id);

        public async Task<VoucharDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var model = await _db.Vouchars.GetById(id);
            await _db.Vouchars.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<VoucharDto>(model);
        }

        public async Task<int> Count(DateTime? from = null, DateTime? to = null)
        {
            return await _db.Vouchars.Count(from, to);
        }
    }
}
