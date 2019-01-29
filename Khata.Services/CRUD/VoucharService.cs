using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Http;

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public class VoucharService : IVoucharService
    {
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public VoucharService(IUnitOfWork db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<Vouchar>> Get(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<Vouchar, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.Supplier.FullName.ToLowerInvariant().Contains(pf.Filter)
                    || p.Supplier.CompanyName.ToLowerInvariant().Contains(pf.Filter)
                    || p.Supplier.Phone.Contains(pf.Filter)
                    || p.Supplier.Email.Contains(pf.Filter);

            return await _db.Vouchars.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
        }

        public async Task<Vouchar> Get(int id)
        {
            return await _db.Vouchars.GetById(id);
        }

        public async Task<Vouchar> Add(Vouchar model)
        {
            model.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Vouchars.Add(model);
            await _db.CompleteAsync();

            return model;
        }

        public async Task<Vouchar> SetPurchase(int invoiceId, int saleId)
        {
            var invoice = await Get(invoiceId);
            invoice.PurchaseId = saleId;
            await _db.CompleteAsync();

            return invoice;
        }

        public async Task<Vouchar> SetSupplierPayment(int invoiceId, int debtPaymentId)
        {
            var invoice = await Get(invoiceId);
            invoice.SupplierPaymentId = debtPaymentId;
            await _db.CompleteAsync();

            return invoice;
        }

        public async Task<Vouchar> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.Vouchars.IsRemoved(id))
                return null;
            await _db.Vouchars.Remove(id);
            await _db.CompleteAsync();
            return await _db.Vouchars.GetById(id);
        }

        public async Task<bool> Exists(int id) => await _db.Vouchars.Exists(id);

        public async Task<Vouchar> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var model = await _db.Vouchars.GetById(id);
            await _db.Vouchars.Delete(id);
            await _db.CompleteAsync();
            return model;
        }
    }
}
