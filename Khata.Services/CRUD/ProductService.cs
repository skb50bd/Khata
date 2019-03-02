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

using Brotal.Extensions;

namespace Khata.Services.CRUD
{
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
            var predicate = string.IsNullOrEmpty(pf.Filter)
                ? (Expression<Func<Product, bool>>)(p => !p.IsRemoved)
                : p => p.Id.ToString() == pf.Filter
                    || p.Outlet.Title.ToLowerInvariant().Contains(pf.Filter)
                    || p.Name.ToLowerInvariant().Contains(pf.Filter);

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
            var originalProduct = await _db.Products.GetById(newProduct.Id);
            var meta = originalProduct.Metadata.Modified(CurrentUser);
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
            await _db.Products.Remove(id);
            await _db.CompleteAsync();
            return _mapper.Map<ProductDto>(await _db.Products.GetById(id));
        }

        public async Task<bool> Exists(int id) => await _db.Products.Exists(id);

        public async Task<ProductDto> Delete(int id)
        {
            if (!(await Exists(id)))
                return null;

            var dto = _mapper.Map<ProductDto>(await _db.Products.GetById(id));
            await _db.Products.Delete(id);
            await _db.CompleteAsync();
            return _mapper.Map<ProductDto>(dto);
        }

        public async Task<int> Count(DateTime? from = null, DateTime? to = null)
        {
            return await _db.Products.Count(from, to);
        }
    }
}