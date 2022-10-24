using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class PurchaseRepository : TrackingRepository<Purchase>, ITrackingRepository<Purchase>
{
    public PurchaseRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }

    public override async Task<IPagedList<Purchase>> Get<T>(
            Expression<Func<Purchase, bool>> predicate,
            Expression<Func<Purchase, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<Purchase>()
            .AsNoTracking()
            .OrderByDescending(order)
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .Include(s => s.Cart)
            .Include(s => s.Supplier)
            .ToPagedListAsync(pageIndex, pageSize);

    public override async Task<Purchase?> GetById(int id) => 
        await Context.Set<Purchase>()
            .Include(s => s.Supplier)
            .Include(s => s.Cart)
            .FirstOrDefaultAsync(s => s.Id == id);
}