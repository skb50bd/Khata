using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Microsoft.AspNetCore.Http;

using Brotal.Extensions;

namespace Khata.Services.CRUD
{
    public class SaleService : ISaleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public SaleService(IMapper mapper,
            IUnitOfWork db,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<SaleDto>> Get(
            int outletId,
            PageFilter pf,
            DateTime? from = null,
            DateTime? to = null)
        {
            var predicate = string.IsNullOrEmpty(pf?.Filter)
                ? (Expression<Func<Sale, bool>>)(s => !s.IsRemoved)
                : s => s.Id.ToString() == pf.Filter
                    || s.InvoiceId.ToString() == pf.Filter
                    || s.Outlet.Title.ToLowerInvariant().Contains(pf.Filter)
                    || s.Customer.FullName.ToLowerInvariant().Contains(pf.Filter);

            if(outletId != 0)
            {
                predicate = predicate.And(s => s.OutletId == outletId);
            }

            var res = await _db.Sales.Get(
                predicate,
                p => p.Metadata.CreationTime,
                pf.PageIndex,
                pf.PageSize,
                from,
                to);
            return res.CastList(c => _mapper.Map<SaleDto>(c));
        }

        public async Task<SaleDto> Get(int id) =>
            _mapper.Map<SaleDto>(await _db.Sales.GetById(id));

        public async Task<IEnumerable<SaleDto>> GetCustomerSales(int customerId)
        {
            var res = await _db.Sales.Get(
                s => s.CustomerId == customerId,
                p => p.Id,
                1,
                int.MaxValue,
                DateTime.Today.AddYears(-1),
                DateTime.Now)
            ;
            return res.CastList(c => _mapper.Map<SaleDto>(c));
        }

        public async Task<SaleDto> Add(SaleViewModel model)
        {
            if (model.Cart is null && model.Payment.Paid == 0)
            {
                throw new Exception("Invalid Operation");
            }

            var dm = _mapper.Map<Sale>(model);
            DebtPayment dp = null;
            CustomerInvoice invoice = null;

            dm.Customer =
                model.RegisterNewCustomer
                    ? _mapper.Map<Customer>(model.Customer)
                    : await _db.Customers.GetById(model.CustomerId);

            if (model.RegisterNewCustomer)
                dm.Customer.Metadata = Metadata.CreatedNew(CurrentUser);

            dm.Cart = new List<SaleLineItem>();
            if (model.Cart?.Count > 0)
            {
                dm.Cart = await Task.WhenAll(
                    model.Cart
                        .Select(async (li) =>
                            li.Type == LineItemType.Product
                                ? await Sold(li.ItemId, li.Quantity, li.NetPrice)
                                : await Sold(li.ItemId, li.NetPrice)));
            }

            dm.SaleDate = (DateTime)model.SaleDate.TryParseDate(DateTime.Now);

            dm.Payment.SubTotal = dm.Cart.Sum(li => li.NetPrice);

            dm.Invoice = _mapper.Map<CustomerInvoice>(dm);
            dm.Invoice.PreviousDue = dm.Customer.Debt;
            dm.Invoice.Metadata = Metadata.CreatedNew(CurrentUser);

            if (dm.Payment.Due < 0)
            {
                dp = new DebtPayment
                {
                    Customer = dm.Customer,
                    Invoice = dm.Invoice,
                    DebtBefore = dm.Customer.Debt,
                    Amount = -dm.Payment.Due,
                    Description = dm.Description,
                    Metadata = Metadata.CreatedNew(CurrentUser)
                };
                dm.Payment.Paid -= dp.Amount;
                dm.Customer.Debt -= dp.Amount;
            }
            dm.Metadata = Metadata.CreatedNew(CurrentUser);


            if (dm.Payment.Due > 0)
            {
                dm.Customer.Debt += dm.Payment.Due;
            }

            invoice = dm.Invoice;
            if (dm.Cart.Count > 0)
            {
                invoice.Sale = dm;
                _db.Sales.Add(dm);
                var deposit = new Deposit(dm as IDeposit)
                {
                    Metadata = Metadata.CreatedNew(CurrentUser)
                };
                _db.Deposits.Add(deposit);
                await _db.CompleteAsync();

                deposit.RowId = dm.RowId;
                await _db.CompleteAsync();

                invoice.SaleId = dm.Id;
            }
            else
            {
                dm.Invoice.Sale = null;
                dm.Invoice = null;
            }

            if (dp != null)
            {
                invoice.DebtPayment = dp;
                _db.DebtPayments.Add(dp);
                var deposit = new Deposit(dp as IDeposit)
                {
                    Metadata = Metadata.CreatedNew(CurrentUser)
                };
                _db.Deposits.Add(deposit);
                await _db.CompleteAsync();

                deposit.RowId = dp.RowId;
                invoice.DebtPaymentId = dp.Id;
            }

            await _db.CompleteAsync();

            return _mapper.Map<SaleDto>(dm);
        }

        public async Task<SaleDto> Update(SaleViewModel vm)
        {
            var newSale = _mapper.Map<Sale>(vm);
            var originalSale = await _db.Sales.GetById(newSale.Id);
            var meta = originalSale.Metadata.Modified(CurrentUser);
            originalSale.SetValuesFrom(newSale);
            originalSale.Metadata = meta;

            await _db.CompleteAsync();

            return _mapper.Map<SaleDto>(originalSale);
        }

        public async Task<SaleDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.Sales.IsRemoved(id))
                return null;
            await _db.Sales.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<SaleDto>(await _db.Sales.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.Sales.Exists(id);

        public async Task<SaleDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<SaleDto>(await _db.Sales.GetById(id));
            await _db.Sales.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<SaleDto>(dto);
        }

        private async Task<SaleLineItem> Sold(
            int productId,
            decimal quantity,
            decimal netPrice)
        {
            var product = await _db.Products.GetById(productId);
            product.Inventory.Stock -= quantity;

            return new SaleLineItem(product, quantity, netPrice);
        }

        private async Task<SaleLineItem> Added(
            int productId,
            decimal quantity,
            decimal netPrice)
        {
            var product = await _db.Products.GetById(productId);
            return new SaleLineItem(product, quantity, netPrice);
        }

        private async Task<SaleLineItem> Sold(
            int serviceId,
            decimal price)
            => new SaleLineItem(await _db.Services.GetById(serviceId), price);

        private async Task<SaleLineItem> Added(
            int serviceId,
            decimal price)
            => await Sold(serviceId, price);

        public async Task<int> Count(DateTime? from = null, DateTime? to = null)
        {
            return await _db.Sales.Count(from, to);
        }

        public async Task<SaleDto> Save(SaleViewModel model)
        {
            if (model.Cart is null)
            {
                throw new Exception("Invalid Operation");
            }

            var dm = _mapper.Map<SavedSale>(model);

            dm.Customer =
                model.RegisterNewCustomer
                    ? _mapper.Map<Customer>(model.Customer)
                    : await _db.Customers.GetById(model.CustomerId);

            if (model.RegisterNewCustomer)
                dm.Customer.Metadata = Metadata.CreatedNew(CurrentUser);

            dm.Cart = new List<SaleLineItem>();
            if (model.Cart?.Count > 0)
            {
                dm.Cart = await Task.WhenAll(
                    model.Cart
                        .Select(async (li) =>
                            li.Type == LineItemType.Product
                                ? await Added(li.ItemId, li.Quantity, li.NetPrice)
                                : await Added(li.ItemId, li.NetPrice)));
            }

            dm.SaleDate = (DateTime)model.SaleDate.TryParseDate(DateTime.Now);
            dm.Payment.SubTotal = dm.Cart.Sum(li => li.NetPrice);
            dm.Metadata = Metadata.CreatedNew(CurrentUser);

            _db.Sales.Save(dm);
            await _db.CompleteAsync();

            return _mapper.Map<SaleDto>(dm);
        }

        public async Task<SaleDto> GetSaved(int id)
            => _mapper.Map<SaleDto>(await _db.Sales.GetSaved(id));

        public async Task<IEnumerable<SaleDto>> GetSaved()
            => (await _db.Sales.GetSaved())
            .Select(s => 
                _mapper.Map<SaleDto>(s))
            .ToList();

        public async Task<SaleDto> DeleteSaved(int id)
        {
            if (await GetSaved(id) is null)
                return null;

            var dto = _mapper.Map<SaleDto>(await _db.Sales.GetSaved(id));
            await _db.Sales.DeleteSaved(id);
            await _db.CompleteAsync();
            return _mapper.Map<SaleDto>(dto);
        }

        public async Task DeleteAllSaved()
            => await _db.Sales.DeleteAllSaved();
    }
}
