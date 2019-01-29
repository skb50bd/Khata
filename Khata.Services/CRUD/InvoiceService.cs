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
    public class InvoiceService : IInvoiceService
    {
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public InvoiceService(IUnitOfWork db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<CustomerInvoice>> Get(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<CustomerInvoice, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.Customer.FullName.ToLowerInvariant().Contains(pf.Filter)
                    || p.Customer.CompanyName.ToLowerInvariant().Contains(pf.Filter)
                    || p.Customer.Phone.Contains(pf.Filter)
                    || p.Customer.Email.Contains(pf.Filter);

            return await _db.Invoices.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
        }

        public async Task<CustomerInvoice> Get(int id)
        {
            return await _db.Invoices.GetById(id);
        }

        public async Task<CustomerInvoice> Add(CustomerInvoice model)
        {
            model.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Invoices.Add(model);
            await _db.CompleteAsync();

            return model;
        }

        public async Task<CustomerInvoice> SetSale(int invoiceId, int saleId)
        {
            var invoice = await Get(invoiceId);
            invoice.SaleId = saleId;
            await _db.CompleteAsync();

            return invoice;
        }

        public async Task<CustomerInvoice> SetDebtPayment(int invoiceId, int debtPaymentId)
        {
            var invoice = await Get(invoiceId);
            invoice.DebtPaymentId = debtPaymentId;
            await _db.CompleteAsync();

            return invoice;
        }

        public async Task<CustomerInvoice> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.Invoices.IsRemoved(id))
                return null;
            await _db.Invoices.Remove(id);
            await _db.CompleteAsync();
            return await _db.Invoices.GetById(id);
        }

        public async Task<bool> Exists(int id) => await _db.Invoices.Exists(id);

        public async Task<CustomerInvoice> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var model = await _db.Invoices.GetById(id);
            await _db.Invoices.Delete(id);
            await _db.CompleteAsync();
            return model;
        }
    }
}
