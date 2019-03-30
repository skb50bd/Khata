using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Brotal.Extensions;

using Data.Core;

using Domain;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories
{
    public class ServiceRepository : TrackingRepository<Service>, ITrackingRepository<Service>
    {
        public ServiceRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<Service>> Get<T>(
            Expression<Func<Service, bool>> predicate,
            Expression<Func<Service, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            predicate = predicate.And(
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? Clock.Min)
                    && i.Metadata.CreationTime <= (to ?? Clock.Max));

            var res = new PagedList<Service>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                    await Context.Services
                        .AsNoTracking()
                        .Where(predicate)
                        .CountAsync()
            };

            res.AddRange(await Context.Services
                .AsNoTracking()
                .Include(s => s.Outlet)
                .Where(predicate)
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<Service> GetById(int id)
            => await Context.Services
            .Include(s => s.Outlet)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
