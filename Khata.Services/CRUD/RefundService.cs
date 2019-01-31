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
    public class RefundService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

        public RefundService(IMapper mapper,
            IUnitOfWork db,
            IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _db = db;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IPagedList<RefundDto>> Get(PageFilter pf)
        {
            var predicate = string.IsNullOrEmpty(pf?.Filter)
                ? (Expression<Func<Refund, bool>>)(s => !s.IsRemoved)
                : s => s.Id.ToString() == pf.Filter
                    || s.SaleId.ToString() == pf.Filter
                    || s.Customer.FullName.ToLowerInvariant().Contains(pf.Filter);

            var res = await _db.Refunds.Get(predicate, p => p.Id, pf.PageIndex, pf.PageSize);
            return res.CastList(c => _mapper.Map<RefundDto>(c));
        }

        public async Task<RefundDto> Get(int id) =>
            _mapper.Map<RefundDto>(await _db.Refunds.GetById(id));

        public async Task<RefundDto> Add(RefundViewModel model)
        {
            if (model.Cart is null
                || (model.CashBack == 0
                && model.DebtRollback == 0))
            {
                throw new Exception("Invalid Operation");
            }

            var dm = _mapper.Map<Refund>(model);

            dm.Customer = await _db.Customers.GetById(model.CustomerId);

            dm.Cart = new List<SaleLineItem>();
            if (model.Cart?.Count > 0)
            {
                dm.Cart = await Task.WhenAll(
                    model.Cart
                        .Select(async (li) =>
                            await Returned(li.ItemId, li.Quantity, li.NetPrice)));
            }

            if (dm.DebtRollback > 0)
            {
                dm.Customer.Debt -= dm.DebtRollback;
            }
            dm.Metadata = Metadata.CreatedNew(CurrentUser);

            if (dm.Cart.Count > 0)
            {
                _db.Refunds.Add(dm);
                if (dm.CashBack > 0)
                {
                    var deposit = new Deposit(dm as IDeposit)
                    {
                        Metadata = Metadata.CreatedNew(CurrentUser)
                    };
                    _db.Deposits.Add(deposit);
                    await _db.CompleteAsync();

                    deposit.RowId = dm.RowId;
                }
                await _db.CompleteAsync();
            }

            await _db.CompleteAsync();

            return _mapper.Map<RefundDto>(dm);
        }

        public async Task<RefundDto> Update(RefundViewModel vm)
        {
            var newRefund = _mapper.Map<Refund>(vm);
            var originalRefund = await _db.Refunds.GetById(newRefund.Id);
            var meta = originalRefund.Metadata.Modified(CurrentUser);
            originalRefund.SetValuesFrom(newRefund);
            originalRefund.Metadata = meta;

            await _db.CompleteAsync();

            return _mapper.Map<RefundDto>(originalRefund);
        }

        public async Task<RefundDto> Remove(int id)
        {
            if (!(await Exists(id))
             || await _db.Refunds.IsRemoved(id))
                return null;
            await _db.Refunds.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<RefundDto>(await _db.Refunds.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.Refunds.Exists(id);

        public async Task<RefundDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<RefundDto>(await _db.Refunds.GetById(id));
            await _db.Refunds.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<RefundDto>(dto);
        }

        private async Task<SaleLineItem> Returned(int productId,
            decimal quantity,
            decimal netPrice)
        {
            var product = await _db.Products.GetById(productId);
            product.Inventory.Stock += quantity;

            return new SaleLineItem(product, quantity, netPrice);
        }
    }
}
