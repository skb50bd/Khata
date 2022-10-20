using System;
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

public class SupplierService : ISupplierService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

    public SupplierService(IUnitOfWork db, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IPagedList<SupplierDto>> Get(
        PageFilter pf,
        DateTime? from = null,
        DateTime? to = null)
    {
        var predicate = string.IsNullOrEmpty(pf.Filter)
            ? (Expression<Func<Supplier, bool>>)(p => true)
            : p => p.Id.ToString() == pf.Filter
                   || p.FullName.ToLowerInvariant().Contains(pf.Filter)
                   || p.CompanyName.ToLowerInvariant().Contains(pf.Filter)
                   || p.Phone.Contains(pf.Filter)
                   || p.Email.Contains(pf.Filter);

        var res = await _db.Suppliers.Get(
            predicate,
            p => p.Id,
            pf.PageIndex,
            pf.PageSize,
            from, to
        );
        return res.CastList(c => _mapper.Map<SupplierDto>(c));
    }

    public async Task<SupplierDto> Get(int id)
    {
        return _mapper.Map<SupplierDto>(await _db.Suppliers.GetById(id));
    }

    public async Task<SupplierDto> Add(SupplierViewModel model)
    {
        var dm = _mapper.Map<Supplier>(model);
        dm.Metadata = Metadata.CreatedNew(CurrentUser);
        _db.Suppliers.Add(dm);
        await _db.CompleteAsync();

        return _mapper.Map<SupplierDto>(dm);
    }

    public async Task<SupplierDto> Update(SupplierViewModel vm)
    {
        var newSupplier = _mapper.Map<Supplier>(vm);
        var originalSupplier = await _db.Suppliers.GetById(newSupplier.Id);
        var meta = originalSupplier.Metadata.Modified(CurrentUser);
        originalSupplier.SetValuesFrom(newSupplier);
        originalSupplier.Metadata = meta;

        await _db.CompleteAsync();

        return _mapper.Map<SupplierDto>(originalSupplier);
    }

    public async Task<SupplierDto> Remove(int id)
    {
        if (!(await Exists(id))
            || await _db.Suppliers.IsRemoved(id))
            return null;
        await _db.Suppliers.Remove(id);
        await _db.CompleteAsync();
        return _mapper.Map<SupplierDto>(await _db.Suppliers.GetById(id));
    }

    public async Task<bool> Exists(int id) => await _db.Suppliers.Exists(id);

    public async Task<SupplierDto> Delete(int id)
    {
        if (!(await Exists(id)))
            return null;

        var dto = _mapper.Map<SupplierDto>(await _db.Suppliers.GetById(id));
        await _db.Suppliers.Delete(id);
        await _db.CompleteAsync();
        return _mapper.Map<SupplierDto>(dto);
    }

    public async Task<int> Count(DateTime? from = null, DateTime? to = null)
    {
        return await _db.Suppliers.Count(from, to);
    }
}