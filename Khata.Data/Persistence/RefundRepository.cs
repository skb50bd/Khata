using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using Microsoft.EntityFrameworkCore;

using Brotal.Extensions;

namespace Khata.Data.Persistence
{
    public class RefundRepository : TrackingRepository<Refund>, ITrackingRepository<Refund>
    {
        public RefundRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<Refund>> Get<T>(
            Expression<Func<Refund, bool>> predicate,
            Expression<Func<Refund, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            predicate = predicate.And(
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                    && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue));

            var res = new PagedList<Refund>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                    await Context.Refunds
                        .AsNoTracking()
                        .Where(predicate)
                        .CountAsync()
            };

            res.AddRange(await Context.Refunds
                .AsNoTracking()
                .Include(s => s.Cart)
                .Include(s => s.Customer)
                .Include(s => s.Metadata)
                .Where(predicate)
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<Refund> GetById(int id)
            => await Context.Refunds
                .Include(s => s.Customer)
                .Include(s => s.Cart)
                .Include(d => d.Metadata)
                .FirstOrDefaultAsync(s => s.Id == id);
    }
}
