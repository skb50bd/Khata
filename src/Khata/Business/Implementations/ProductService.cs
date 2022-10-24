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

public class ProductService : IProductService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _db;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string CurrentUser => _httpContextAccessor.HttpContext.User.Identity.Name;

    public ProductService(IMapper mapper, IUnitOfWork db, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IPagedList<ProductDto>> Get(
        int outletId,
        PageFilter pf,
        DateTime? from = null,
        DateTime? to = null)
    {
        int id;
        Expression<Func<Product, bool>> fuzzySearch =
            p => p.Id.ToString() == pf.Filter
                 || p.Name.ToLowerInvariant().Contains(pf.Filter);
        Expression<Func<Product, bool>> strictSearch =
            p => (int.TryParse(pf.Filter, out id) && p.Id == id)
                 || p.Name.ToLowerInvariant().StartsWith(pf.Filter);

        var predicate = string.IsNullOrEmpty(pf.Filter)
            ? (p => !p.IsRemoved)
            : strictSearch;

        if (outletId != 0)
        {
            predicate = predicate.And(p => p.OutletId == outletId);
        }

        var res = await _db.Products.Get(
            predicate,
            p => p.Id,
            pf.PageIndex,
            pf.PageSize,
            from, to
        );
        return res.CastList(c => _mapper.Map<ProductDto>(c));
    }

    public async Task<ProductDto> Get(int id) =>
        _mapper.Map<ProductDto>(await _db.Products.GetById(id));

    public async Task<ProductDto> Add(ProductViewModel model)
    {
        var dm = _mapper.Map<Product>(model);
        dm.Metadata = Metadata.CreatedNew(CurrentUser);
        _db.Products.Add(dm);
        await _db.CompleteAsync();

        return _mapper.Map<ProductDto>(dm);
    }

    public async Task<ProductDto> Update(ProductViewModel vm)
    {
        var newProduct = _mapper.Map<Product>(vm);
        newProduct.Outlet = await _db.Outlets.GetById(vm.OutletId);
        var originalProduct = await _db.Products.GetById(newProduct.Id);
        var meta = originalProduct.Metadata.ModifiedBy(CurrentUser);
        originalProduct.SetValuesFrom(newProduct);
        originalProduct.Metadata = meta;

        await _db.CompleteAsync();

        return _mapper.Map<ProductDto>(originalProduct);
    }

    public async Task<ProductDto> Remove(int id)
    {
        if (!(await Exists(id))
            || await _db.Products.IsRemoved(id))
            return null;
        var product = await _db.Products.GetById(id);
        await _db.Products.Remove(id);
        await _db.CompleteAsync();
        var dto = _mapper.Map<ProductDto>(product);
        dto.Outlet = null;

        return dto;
    }

    public async Task<bool> Exists(int id) => await _db.Products.Exists(id);

    public async Task<ProductDto> Delete(int id)
    {
        if (!(await Exists(id)))
            return null;

        var dto = _mapper.Map<ProductDto>(await _db.Products.GetById(id));
        await _db.Products.Delete(id);
        await _db.CompleteAsync();
        dto.Outlet = null;

        return dto;
    }

    public async Task<int> Count(DateTime? from = null, DateTime? to = null)
    {
        return await _db.Products.Count(from, to);
    }
}