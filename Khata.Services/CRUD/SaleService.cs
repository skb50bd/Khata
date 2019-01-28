﻿using System;
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

using SharedLibrary;

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

        public async Task<IPagedList<SaleDto>> Get(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf?.Filter)
                ? (Expression<Func<Sale, bool>>)(s => !s.IsRemoved)
                : s => s.Id.ToString() == pf.Filter
                    || s.InvoiceId.ToString() == pf.Filter
                    || s.Customer.FullName.ToLowerInvariant().Contains(pf.Filter);

            var res = await _db.Sales.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
            return res.CastList(c => _mapper.Map<SaleDto>(c));
        }

        public async Task<SaleDto> Get(int id) =>
            _mapper.Map<SaleDto>(await _db.Sales.GetById(id));

        public async Task<SaleDto> Add(SaleViewModel model)
        {
            if (model.Cart?.Count == 0 || model.Payment.Paid == 0)
            {
                throw new Exception("Invalid Operation");
            }

            var dm = _mapper.Map<Sale>(model);
            DebtPayment dp = null;
            Invoice invoice = null;

            dm.Customer =
                model.RegisterNewCustomer
                    ? _mapper.Map<Customer>(model.Customer)
                    : await _db.Customers.GetById(model.CustomerId);

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

            dm.SaleDate =
                DateTimeOffset.ParseExact(
                    model.SaleDate,
                    @"dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat
                );

            dm.Payment.SubTotal = dm.Cart.Sum(li => li.NetPrice);

            dm.Invoice = _mapper.Map<Invoice>(dm);
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
                dm.Payment.Paid += -dm.Payment.Due;
            }
            dm.Customer.Debt += dm.Payment.Due;
            dm.Metadata = Metadata.CreatedNew(CurrentUser);

            invoice = dm.Invoice;
            if (dm.Cart.Count > 0)
            {
                invoice.Sale = dm;
                _db.Sales.Add(dm);
                var deposit = new Deposit(dm as IDeposit)
                {
                    Metadata = Metadata.CreatedNew(CurrentUser)
                };
                await _db.CompleteAsync();

                deposit.RowId = dm.RowId;
                await _db.CompleteAsync();
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
                var deposit2 = new Deposit(dp as IDeposit)
                {
                    Metadata = Metadata.CreatedNew(CurrentUser)
                };
                _db.Deposits.Add(deposit2);
                await _db.CompleteAsync();

                deposit2.RowId = dp.RowId;
                await _db.CompleteAsync();
            }

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

        private async Task<SaleLineItem> Sold(int productId,
            decimal quantity,
            decimal netPrice)
        {
            var product = await _db.Products.GetById(productId);
            product.Inventory.Stock -= quantity;

            return new SaleLineItem(product, quantity, netPrice);
        }

        private async Task<SaleLineItem> Sold(int serviceId,
            decimal price)
            => new SaleLineItem(await _db.Services.GetById(serviceId), price);
    }
}
