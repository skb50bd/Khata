using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class ServiceRepository : TrackingRepository<Service>, ITrackingRepository<Service>
{
    public ServiceRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime)
    { }

    public override async Task<IPagedList<Service>> Get<T>(
            Expression<Func<Service, bool>> predicate,
            Expression<Func<Service, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<Service>()
            .AsNoTracking()
            .OrderByDescending(order)
            .Where(
                predicate.And(i =>
                    i.IsRemoved == false
                    && (from == null || i.Metadata.CreationTime >= from)
                    && (to == null || i.Metadata.CreationTime <= to)
                )
            )
            .Include(s => s.Outlet)
            .ToPagedListAsync(pageIndex, pageSize);

    public override async Task<Service?> GetById(int id) => 
        await Context.Set<Service>()
            .Include(s => s.Outlet)
            .FirstOrDefaultAsync(s => s.Id == id);
}