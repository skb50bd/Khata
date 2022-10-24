using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class ProductRepository : TrackingRepository<Product>, ITrackingRepository<Product>
{
    public ProductRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }

    public override async Task<IPagedList<Product>> Get<T>(
            Expression<Func<Product, bool>> predicate,
            Expression<Func<Product, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<Product>()
            .AsNoTracking()
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .Include(s => s.Outlet)
            .OrderByDescending(order)
            .ToPagedListAsync(pageIndex, pageSize);

    public override async Task<Product?> GetById(int id) => 
        await Context.Set<Product>()
            .Include(s => s.Outlet)
            .FirstOrDefaultAsync(s => s.Id == id);
}