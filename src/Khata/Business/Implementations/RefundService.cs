using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Brotal;
using Brotal.Extensions;
using Business.Abstractions;
using Business.PageFilterSort;
using Data.Core;
using Domain;
using DTOs;
using Microsoft.AspNetCore.Http;
using ViewModels;
using Metadata = Domain.Metadata;

namespace Business.Implementations;

public class RefundService : IRefundService
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

    public async Task<IPagedList<RefundDto>> Get(
        PageFilter pf,
        DateTime? from = null,
        DateTime? to = null)
    {
        var predicate = string.IsNullOrEmpty(pf?.Filter)
            ? (Expression<Func<Refund, bool>>)(s => !s.IsRemoved)
            : s => s.Id.ToString() == pf.Filter
                   || s.SaleId.ToString() == pf.Filter
                   || s.Customer.FullName.ToLowerInvariant().Contains(pf.Filter);

        var res = await _db.Refunds.Get(
            predicate,
            p => p.Metadata.CreationTime,
            pf.PageIndex,
            pf.PageSize,
            from, to
        );
        return res.CastList(c => _mapper.Map<RefundDto>(c));
    }

    public async Task<RefundDto> Get(int id) =>
        _mapper.Map<RefundDto>(await _db.Refunds.GetById(id));

    public async Task<RefundDto> Add(RefundViewModel model)
    {
        if (model.Cart is null)
        {
            throw new Exception("Invalid Operation");
        }

        var dm = _mapper.Map<Refund>(model);

        var sale = await _db.Sales.GetById(model.SaleId);
        dm.Customer = await _db.Customers.GetById(sale.CustomerId);

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
                var withdrawal = new Withdrawal(dm as IWithdrawal)
                {
                    Metadata = Metadata.CreatedNew(CurrentUser)
                };
                _db.Withdrawals.Add(withdrawal);
                await _db.CompleteAsync();

                withdrawal.RowId = dm.RowId;
            }
            await _db.CompleteAsync();
        }

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

    public async Task<int> Count(DateTime? from = null, DateTime? to = null)
    {
        return await _db.Refunds.Count(from, to);
    }

    public async Task<IEnumerable<RefundDto>> GetCustomerRefunds(
        int customerId,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        var res = await _db.Refunds.Get(
            s => s.CustomerId == customerId,
            p => p.Id,
            1,
            int.MaxValue,
            fromDate ?? Clock.Today.AddYears(-1),
            toDate ?? Clock.Now);
        ;
        return res.CastList(c => _mapper.Map<RefundDto>(c));
    }
}