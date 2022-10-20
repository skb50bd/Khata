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

public class PurchaseReturnService : IPurchaseReturnService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

    public PurchaseReturnService(IMapper mapper,
        IUnitOfWork db,
        IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IPagedList<PurchaseReturnDto>> Get(
        PageFilter pf,
        DateTime? from = null,
        DateTime? to = null)
    {
        var predicate = string.IsNullOrEmpty(pf?.Filter)
            ? (Expression<Func<PurchaseReturn, bool>>)(s => !s.IsRemoved)
            : s => s.Id.ToString() == pf.Filter
                   || s.PurchaseId.ToString() == pf.Filter
                   || s.Supplier.FullName.ToLowerInvariant().Contains(pf.Filter);

        var res = await _db.PurchaseReturns.Get(
            predicate,
            p => p.Metadata.CreationTime,
            pf.PageIndex,
            pf.PageSize,
            from, to
        );
        return res.CastList(c => _mapper.Map<PurchaseReturnDto>(c));
    }

    public async Task<PurchaseReturnDto> Get(int id) =>
        _mapper.Map<PurchaseReturnDto>(await _db.PurchaseReturns.GetById(id));

    public async Task<PurchaseReturnDto> Add(PurchaseReturnViewModel model)
    {
        if (model.Cart is null)
        {
            throw new Exception("Invalid Operation");
        }

        var dm = _mapper.Map<PurchaseReturn>(model);

        var purchase = await _db.Purchases.GetById(model.PurchaseId);
        dm.Supplier = await _db.Suppliers.GetById(purchase.SupplierId);

        dm.Cart = new List<PurchaseLineItem>();
        if (model.Cart?.Count > 0)
        {
            dm.Cart = await Task.WhenAll(
                model.Cart
                    .Select(async (li) =>
                        await Returned(li.ItemId, li.Quantity, li.NetPrice)));
        }

        if (dm.DebtRollback > 0)
        {
            dm.Supplier.Payable -= dm.DebtRollback;
        }
        dm.Metadata = Metadata.CreatedNew(CurrentUser);

        if (dm.Cart.Count > 0)
        {
            _db.PurchaseReturns.Add(dm);
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

        return _mapper.Map<PurchaseReturnDto>(dm);
    }

    public async Task<PurchaseReturnDto> Update(PurchaseReturnViewModel vm)
    {
        var newPurchaseReturn = _mapper.Map<PurchaseReturn>(vm);
        var originalPurchaseReturn = await _db.PurchaseReturns.GetById(newPurchaseReturn.Id);
        var meta = originalPurchaseReturn.Metadata.Modified(CurrentUser);
        originalPurchaseReturn.SetValuesFrom(newPurchaseReturn);
        originalPurchaseReturn.Metadata = meta;

        await _db.CompleteAsync();

        return _mapper.Map<PurchaseReturnDto>(originalPurchaseReturn);
    }

    public async Task<PurchaseReturnDto> Remove(int id)
    {
        if (!(await Exists(id))
            || await _db.PurchaseReturns.IsRemoved(id))
            return null;
        await _db.PurchaseReturns.Remove(id);
        await _db.CompleteAsync();
        return _mapper.Map<PurchaseReturnDto>(await _db.PurchaseReturns.GetById(id));
    }

    public async Task<bool> Exists(int id) => await _db.PurchaseReturns.Exists(id);

    public async Task<PurchaseReturnDto> Delete(int id)
    {
        if (!(await Exists(id)))
            return null;

        var dto = _mapper.Map<PurchaseReturnDto>(await _db.PurchaseReturns.GetById(id));
        await _db.PurchaseReturns.Delete(id);
        await _db.CompleteAsync();
        return _mapper.Map<PurchaseReturnDto>(dto);
    }

    private async Task<PurchaseLineItem> Returned(int productId,
        decimal quantity,
        decimal netPrice)
    {
        var product = await _db.Products.GetById(productId);
        product.Inventory.Stock -= quantity;

        return new PurchaseLineItem(product, quantity, netPrice);
    }

    public async Task<int> Count(DateTime? from = null, DateTime? to = null)
    {
        return await _db.PurchaseReturns.Count(from, to);
    }

    public async Task<IEnumerable<PurchaseReturnDto>> GetSupplierPurchaseReturns(int supplierId)
    {
        var res = await _db.PurchaseReturns.Get(
            s => s.SupplierId == supplierId,
            p => p.Id,
            1,
            int.MaxValue,
            Clock.Today.AddYears(-1),
            Clock.Now);
        return res.CastList(c => _mapper.Map<PurchaseReturnDto>(c));
    }
}