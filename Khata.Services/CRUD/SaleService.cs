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

using SharedLibrary;

namespace Khata.Services.CRUD
{
    public class SaleService : ISaleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDebtPaymentService _debtPayments;
        private readonly ICashRegisterService _cashRegister;
        private readonly IProductService _products;
        private readonly IServiceService _services;
        private readonly IInvoiceService _invoices;
        private readonly ICustomerService _customers;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public SaleService(IMapper mapper,
            IUnitOfWork db,
            IProductService products,
            IServiceService services,
            IDebtPaymentService debtPayments,
            IInvoiceService invoices,
            ICashRegisterService cashRegister,
            ICustomerService customers,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _db = db;
            _products = products;
            _services = services;
            _debtPayments = debtPayments;
            _invoices = invoices;
            _cashRegister = cashRegister;
            _customers = customers;
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
            CustomerDto customerDto;
            if (model.RegisterNewCustomer)
            {
                customerDto = await _customers.Add(model.Customer);
                model.CustomerId = customerDto.Id;
            }
            else
            {
                customerDto = await _customers.Get(model.CustomerId);
            }
            model.Customer = _mapper.Map<CustomerViewModel>(customerDto);

            ICollection<SaleLineItem> cart = new List<SaleLineItem>();
            if (model.Cart?.Count > 0)
            {
                cart = await Task.WhenAll(
                model.Cart
                    .Select(async (li) =>
                        li.Type == LineItemType.Product
                            ? await Sold(li.ItemId, li.Quantity, li.NetPrice)
                            : await Sold(li.ItemId, li.NetPrice)));
            }

            var saleDm = _mapper.Map<Sale>(model);
            saleDm.SaleDate =
                DateTimeOffset.ParseExact(
                    model.SaleDate,
                    @"dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat
                );
            saleDm.Cart = cart;
            saleDm.Payment.SubTotal = saleDm.Cart.Sum(li => li.NetPrice);

            var invoice = _mapper.Map<Invoice>(model);
            saleDm.Cart
                .ForEach(
                    li => invoice.Cart
                            .Add(_mapper.Map<InvoiceLineItem>(li)));

            invoice = await _invoices.Add(invoice);

            if (saleDm.Payment.Due < 0)
            {
                if (customerDto != null)
                {
                    var dp = new DebtPaymentViewModel
                    {
                        CustomerId = customerDto.Id,
                        InvoiceId = invoice.Id,
                        Amount = -saleDm.Payment.Due
                    };
                    await _debtPayments.Add(dp);
                    saleDm.Payment.Paid -= dp.Amount;
                }
            }

            saleDm.Metadata = Metadata.CreatedNew(CurrentUser);
            saleDm.InvoiceId = invoice.Id;
            _db.Sales.Add(saleDm);
            await _db.CompleteAsync();

            await _cashRegister.AddDeposit(saleDm);
            await _invoices.SetSale(invoice.Id, saleDm.Id);

            return _mapper.Map<SaleDto>(saleDm);
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
