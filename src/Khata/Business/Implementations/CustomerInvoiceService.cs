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

namespace Business.Implementations
{
    public class InvoiceService : ICustomerInvoiceService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public InvoiceService(
            IUnitOfWork db,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<CustomerInvoiceDto>> Get(PageFilter pf,
            DateTime? from = null,
            DateTime? to = null)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<CustomerInvoice, bool>>)(p => true)
                : p => p.Id.ToString() == pf.Filter
                    || p.Customer.FullName.ToLowerInvariant().Contains(pf.Filter)
                    || p.Customer.CompanyName.ToLowerInvariant().Contains(pf.Filter)
                    || p.Customer.Phone.Contains(pf.Filter)
                    || p.Customer.Email.Contains(pf.Filter);
            var res = await _db.Invoices.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize, from, to);
            return res.CastList(c => _mapper.Map<CustomerInvoiceDto>(c));
        }

        public async Task<CustomerInvoiceDto> Get(int id)
        {
            return _mapper.Map<CustomerInvoiceDto>(await _db.Invoices.GetById(id));
        }

        public async Task<CustomerInvoiceDto> Add(CustomerInvoice model)
        {
            model.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Invoices.Add(model);
            await _db.CompleteAsync();

            return _mapper.Map<CustomerInvoiceDto>(model);
        }

        public async Task<CustomerInvoiceDto> SetSale(
            int invoiceId,
            int saleId)
        {
            var invoice = await _db.Invoices.GetById(invoiceId);
            invoice.SaleId = saleId;
            await _db.CompleteAsync();

            return _mapper.Map<CustomerInvoiceDto>(invoice);
        }

        public async Task<CustomerInvoiceDto> SetDebtPayment(
            int invoiceId,
            int debtPaymentId)
        {
            var invoice = await _db.Invoices.GetById(invoiceId);
            invoice.DebtPaymentId = debtPaymentId;
            await _db.CompleteAsync();

            return _mapper.Map<CustomerInvoiceDto>(invoice);
        }

        public async Task<CustomerInvoiceDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.Invoices.IsRemoved(id))
                return null;
            await _db.Invoices.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<CustomerInvoiceDto>(await _db.Invoices.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.Invoices.Exists(id);

        public async Task<CustomerInvoiceDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var model = await _db.Invoices.GetById(id);
            await _db.Invoices.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<CustomerInvoiceDto>(model);
        }

        public async Task<int> Count(DateTime? from = null, DateTime? to = null)
        {
            return await _db.Invoices.Count(from, to);
        }
    }
}
