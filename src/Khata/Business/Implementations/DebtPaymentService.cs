using System;
using System.Collections.Generic;
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

public class DebtPaymentService : IDebtPaymentService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

    public DebtPaymentService(IUnitOfWork db,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IPagedList<DebtPaymentDto>> Get(
        PageFilter pf,
        DateTime? from = null,
        DateTime? to = null)
    {
        var predicate = string.IsNullOrEmpty(pf.Filter)
            ? (Expression<Func<DebtPayment, bool>>)(p => true)
            : p => p.Id.ToString() == pf.Filter
                   || p.Customer.FullName
                       .ToLowerInvariant()
                       .Contains(pf.Filter);

        var res =
            await _db.DebtPayments.Get(
                predicate,
                p => p.Id,
                pf.PageIndex,
                pf.PageSize,
                from,
                to
            );

        return res.CastList(c => _mapper.Map<DebtPaymentDto>(c));
    }

    public async Task<DebtPaymentDto> Get(int id)
    {
        return _mapper.Map<DebtPaymentDto>(await _db.DebtPayments.GetById(id));
    }

    public async Task<DebtPaymentDto> Add(
        DebtPaymentViewModel model)
    {
        var dm = _mapper.Map<DebtPayment>(model);
        dm.Customer = await _db.Customers.GetById(model.CustomerId);
        dm.DebtBefore = dm.Customer.Debt;
        dm.Customer.Debt -= dm.Amount;

        dm.Invoice = _mapper.Map<CustomerInvoice>(dm);
        dm.Invoice.Metadata = Metadata.CreatedNew(CurrentUser);
        dm.Invoice.DebtPayment = dm;
        dm.Metadata = Metadata.CreatedNew(CurrentUser);

        var deposit = new Deposit(dm)
        {
            Metadata = Metadata.CreatedNew(CurrentUser)
        };
        _db.DebtPayments.Add(dm);
        _db.Deposits.Add(deposit);

        await _db.CompleteAsync();

        deposit.RowId = dm.RowId;
        await _db.CompleteAsync();

        return _mapper.Map<DebtPaymentDto>(dm);
    }

    public async Task<DebtPaymentDto> Update(
        DebtPaymentViewModel vm)
    {
        var newDebtPayment = _mapper.Map<DebtPayment>(vm);
        var originalDebtPayment = await _db.DebtPayments.GetById(newDebtPayment.Id);
        var meta = originalDebtPayment.Metadata.Modified(CurrentUser);
        originalDebtPayment.SetValuesFrom(newDebtPayment);
        originalDebtPayment.Metadata = meta;

        await _db.CompleteAsync();

        return _mapper.Map<DebtPaymentDto>(originalDebtPayment);
    }

    public async Task<DebtPaymentDto> Remove(int id)
    {
        if (!(await Exists(id))
            || await _db.DebtPayments.IsRemoved(id))
            return null;
        await _db.DebtPayments.Remove(id);
        await _db.CompleteAsync();
        return _mapper.Map<DebtPaymentDto>(await _db.DebtPayments.GetById(id));
    }

    public async Task<bool> Exists(int id) => await _db.DebtPayments.Exists(id);

    public async Task<DebtPaymentDto> Delete(int id)
    {
        if (!(await Exists(id)))
            return null;

        var dto = _mapper.Map<DebtPaymentDto>(await _db.DebtPayments.GetById(id));
        await _db.DebtPayments.Delete(id);
        await _db.CompleteAsync();
        return _mapper.Map<DebtPaymentDto>(dto);
    }

    public async Task<int> Count(DateTime? from = null, DateTime? to = null)
    {
        return await _db.DebtPayments.Count(from, to);
    }

    public async Task<IEnumerable<DebtPaymentDto>> GetCustomerDebtPayments(
        int customerId,
        DateTime? fromDate = null,
        DateTime? toDate = null)
    {
        var res = await _db.DebtPayments.Get(
            s => s.CustomerId == customerId,
            p => p.Id,
            1,
            int.MaxValue,
            fromDate ?? Clock.Today.AddYears(-1),
            toDate ?? Clock.Now);
        ;
        return res.CastList(c => _mapper.Map<DebtPaymentDto>(c));
    }
}