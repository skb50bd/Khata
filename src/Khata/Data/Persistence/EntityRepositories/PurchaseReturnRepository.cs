using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class PurchaseReturnRepository : TrackingRepository<PurchaseReturn>, ITrackingRepository<PurchaseReturn>
{
    public PurchaseReturnRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }

    public override async Task<IPagedList<PurchaseReturn>> Get<T>(
            Expression<Func<PurchaseReturn, bool>> predicate,
            Expression<Func<PurchaseReturn, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<PurchaseReturn>()
            .AsNoTracking()
            .OrderByDescending(order)
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .Include(s => s.Cart)
            .Include(s => s.Supplier)
            .ToPagedListAsync(pageIndex, pageSize);

    public override async Task<PurchaseReturn?> GetById(int id) => 
        await Context.Set<PurchaseReturn>()
            .Include(s => s.Supplier)
            .Include(s => s.Cart)
            .FirstOrDefaultAsync(s => s.Id == id);
}