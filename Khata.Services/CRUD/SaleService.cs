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
    public class SaleService : ISaleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDebtPaymentService _debtPayments;
        private readonly IProductService _products;
        private readonly ICustomerService _customers;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public SaleService(IMapper mapper,
            IUnitOfWork db,
            IProductService products,
            IDebtPaymentService debtPayments,
            ICustomerService customers,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _db = db;
            _products = products;
            _debtPayments = debtPayments;
            _customers = customers;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<SaleDto>> Get(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<Sale, bool>>)(s => true)
                : s => s.Id.ToString() == pf.Filter
                    || s.Customer.FullName.ToLowerInvariant().Contains(pf.Filter);

            var res = await _db.Sales.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
            return res.CastList(c => _mapper.Map<SaleDto>(c));
        }

        public async Task<SaleDto> Get(int id) =>
            _mapper.Map<SaleDto>(await _db.Sales.GetById(id));

        public async Task<SaleDto> Add(SaleViewModel model)
        {
            if (model.Payment.Due < 0)
            {
                var cus = await _customers.Get(model.CustomerId);
                if (cus != null)
                {
                    var dp = new DebtPaymentViewModel
                    {
                        CustomerId = cus.Id,
                        Amount = -1 * model.Payment.Due
                    };
                    await _debtPayments.Add(dp);
                    model.Payment.Paid -= dp.Amount;
                }
            }

            foreach (var item in model.Cart)
            {
                if (item.Type == LineItemType.Product)
                {
                    var p = _mapper.Map<ProductViewModel>(await _products.Get(item.ItemId));
                    p.InventoryStock -= item.Quantity;
                    await _products.Update(p);
                }
            }

            var dm = _mapper.Map<Sale>(model);
            dm.Metadata = Metadata.CreatedNew(CurrentUser);
            _db.Sales.Add(dm);
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

    }
}
