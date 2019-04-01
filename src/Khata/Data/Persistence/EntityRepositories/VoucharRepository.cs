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
    public class VoucharRepository : TrackingRepository<Vouchar>, ITrackingRepository<Vouchar>
    {
        public VoucharRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<Vouchar>> Get<T>(
            Expression<Func<Vouchar, bool>> predicate,
            Expression<Func<Vouchar, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            predicate = predicate.And(
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? Clock.Min)
                    && i.Metadata.CreationTime <= (to ?? Clock.Max));

            var res = new PagedList<Vouchar>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                    await Context.Vouchars
                        .AsNoTracking()
                        .Where(predicate)
                        .CountAsync()
            };

            res.AddRange(await Context.Vouchars
                .AsNoTracking()
                .Include(s => s.Supplier)
                .Where(predicate)
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<Vouchar> GetById(int id)
            => await Context.Vouchars
            .Include(s => s.Supplier)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
