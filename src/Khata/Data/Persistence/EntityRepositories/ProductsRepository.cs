using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Brotal;
using Brotal.Extensions;

using Data.Core;

using Domain;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class ProductRepository : TrackingRepository<Product>, ITrackingRepository<Product>
{
    public ProductRepository(KhataContext context) : base(context) { }

    public override async Task<IPagedList<Product>> Get<T>(
        Expression<Func<Product, bool>> predicate,
        Expression<Func<Product, T>> order,
        int pageIndex,
        int pageSize,
        DateTime? from = null,
        DateTime? to = null)
    {
        predicate = predicate.And(
            i => !i.IsRemoved
                 && i.Metadata.CreationTime >= (from ?? Clock.Min)
                 && i.Metadata.CreationTime <= (to ?? Clock.Max));

        var res = new PagedList<Product>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            ResultCount =
                await Context.Products
                    .AsNoTracking()
                    .Where(predicate)
                    .CountAsync()
        };

        res.AddRange(await Context.Products
            .AsNoTracking()
            .Include(s => s.Outlet)
            .Where(predicate)
            .OrderByDescending(order)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize > 0 ? pageSize : int.MaxValue)
            .ToListAsync());

        return res;
    }

    public override async Task<Product> GetById(int id)
        => await Context.Products
            .Include(s => s.Outlet)
            .FirstOrDefaultAsync(s => s.Id == id);
}