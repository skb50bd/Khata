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

public class PurchaseService : IPurchaseService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

    public PurchaseService(IMapper mapper,
        IUnitOfWork db,
        IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IPagedList<PurchaseDto>> Get(
        PageFilter pf,
        DateTime? from = null,
        DateTime? to = null)
    {
        var predicate = string.IsNullOrEmpty(pf?.Filter)
            ? (Expression<Func<Purchase, bool>>)(s => !s.IsRemoved)
            : s => s.Id.ToString() == pf.Filter
                   || s.VoucharId.ToString() == pf.Filter
                   || s.Supplier.FullName.ToLowerInvariant().Contains(pf.Filter);

        var res = await _db.Purchases.Get(
            predicate,
            p => p.Metadata.CreationTime,
            pf.PageIndex,
            pf.PageSize,
            from, to
        );
        return res.CastList(c => _mapper.Map<PurchaseDto>(c));
    }

    public async Task<PurchaseDto> Get(int id) =>
        _mapper.Map<PurchaseDto>(await _db.Purchases.GetById(id));

    public async Task<PurchaseDto> Add(PurchaseViewModel model)
    {
        if (model.Cart is null && model.Payment.Paid == 0)
        {
            throw new Exception("Invalid Operation");
        }

        var dm = _mapper.Map<Purchase>(model);
        SupplierPayment sp = null;

        dm.Supplier =
            model.RegisterNewSupplier
                ? _mapper.Map<Supplier>(model.Supplier)
                : await _db.Suppliers.GetById(model.SupplierId);
        if (model.RegisterNewSupplier)
            dm.Supplier.Metadata = Metadata.CreatedNew(CurrentUser);

        dm.Cart = new List<PurchaseCartItem>();
        if (model.Cart?.Count > 0)
        {
            dm.Cart = await Task.WhenAll(
                model.Cart
                    .Select(async li => await Purchased(li.ItemId, li.Quantity, li.NetPrice)));
        }

        dm.PurchaseDate =
            DateTimeOffset.ParseExact(
                model.PurchaseDate,
                @"dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat
            );

        dm.Payment.SubTotal = dm.Cart.Sum(li => li.NetPurchasePrice);

        dm.Vouchar = _mapper.Map<Vouchar>(dm);
        dm.Vouchar.PreviousDue = dm.Supplier.Payable;
        dm.Vouchar.Metadata = Metadata.CreatedNew(CurrentUser);

        if (dm.Payment.Due < 0)
        {
            sp = new SupplierPayment
            {
                PaymentDate = dm.PurchaseDate,
                Supplier = dm.Supplier,
                Vouchar = dm.Vouchar,
                PayableBefore = dm.Supplier.Payable,
                Amount = -dm.Payment.Due,
                Description = dm.Description,
                Metadata = Metadata.CreatedNew(CurrentUser)
            };
            dm.Payment.Paid -= sp.Amount;
            dm.Supplier.Payable -= sp.Amount;
        }
        dm.Metadata = Metadata.CreatedNew(CurrentUser);

        if (dm.Payment.Due > 0)
        {
            dm.Supplier.Payable += dm.Payment.Due;
        }
        var vouchar = dm.Vouchar;
        if (dm.Cart.Count > 0)
        {
            vouchar.Purchase = dm;
            _db.Purchases.Add(dm);
            var withdrawal = new Withdrawal(dm as IWithdrawal)
            {
                Metadata = Metadata.CreatedNew(CurrentUser)
            };
            _db.Withdrawals.Add(withdrawal);
            await _db.CompleteAsync();

            withdrawal.RowId = dm.RowId;
            await _db.CompleteAsync();

            vouchar.PurchaseId = dm.Id;
        }
        else
        {
            dm.Vouchar.Purchase = null;
            dm.Vouchar = null;
        }

        if (sp != null)
        {
            vouchar.SupplierPayment = sp;
            _db.SupplierPayments.Add(sp);
            var withdrawal = new Withdrawal(sp as IWithdrawal)
            {
                Metadata = Metadata.CreatedNew(CurrentUser)
            };
            _db.Withdrawals.Add(withdrawal);
            await _db.CompleteAsync();

            withdrawal.RowId = sp.RowId;
            vouchar.SupplierPaymentId = sp.Id;
        }

        await _db.CompleteAsync();

        return _mapper.Map<PurchaseDto>(dm);
    }

    public async Task<PurchaseDto> Update(PurchaseViewModel vm)
    {
        var newPurchase = _mapper.Map<Purchase>(vm);
        var originalPurchase = await _db.Purchases.GetById(newPurchase.Id);
        var meta = originalPurchase.Metadata.ModifiedBy(CurrentUser);
        originalPurchase.SetValuesFrom(newPurchase);
        originalPurchase.Metadata = meta;

        await _db.CompleteAsync();

        return _mapper.Map<PurchaseDto>(originalPurchase);
    }

    public async Task<PurchaseDto> Remove(int id)
    {
        if (!(await Exists(id))
            || await _db.Purchases.IsRemoved(id))
            return null;
        await _db.Purchases.Remove(id);
        await _db.CompleteAsync();
        return _mapper.Map<PurchaseDto>(await _db.Purchases.GetById(id));
    }

    public async Task<bool> Exists(int id) => await _db.Purchases.Exists(id);

    public async Task<PurchaseDto> Delete(int id)
    {
        if (!(await Exists(id)))
            return null;

        var dto = _mapper.Map<PurchaseDto>(await _db.Purchases.GetById(id));
        await _db.Purchases.Delete(id);
        await _db.CompleteAsync();
        return _mapper.Map<PurchaseDto>(dto);
    }

    private async Task<PurchaseCartItem> Purchased(int productId,
        decimal quantity,
        decimal netPrice)
    {
        var product = await _db.Products.GetById(productId);
        var previousStock = product.Inventory.TotalStock;
        var previousTotalPrice = product.Price.Purchase * previousStock;
        var newPurchasePrice = (previousTotalPrice + netPrice) / (previousStock + quantity);
        product.Inventory.Stock += quantity;
        product.Price.Purchase = newPurchasePrice;

        return new PurchaseCartItem(product, quantity, netPrice);
    }

    public async Task<int> Count(DateTime? from = null, DateTime? to = null)
    {
        return await _db.Purchases.Count(from, to);
    }

    public async Task<IEnumerable<PurchaseDto>> GetSupplierPurchases(int supplierId)
    {
        var res = await _db.Purchases.Get(
            s => s.SupplierId == supplierId,
            p => p.Id,
            1,
            int.MaxValue,
            Clock.Today.AddYears(-1),
            Clock.Now);
        return res.CastList(c => _mapper.Map<PurchaseDto>(c));
    }
}