using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class RefundRepository : TrackingRepository<Refund>
{
    public RefundRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }

    public override async Task<IPagedList<Refund>> Get<T>(
            Expression<Func<Refund, bool>> predicate,
            Expression<Func<Refund, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<Refund>()
            .AsNoTracking()
            .OrderByDescending(order)
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .Include(s => s.Cart)
            .Include(s => s.Customer)
            .ToPagedListAsync(pageIndex, pageSize);

    public override async Task<Refund?> GetById(int id)
        => await Context.Set<Refund>()
            .Include(s => s.Customer)
            .Include(s => s.Cart)
            .FirstOrDefaultAsync(s => s.Id == id);
}