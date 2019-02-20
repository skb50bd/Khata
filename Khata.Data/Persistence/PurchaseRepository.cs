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
    public class PurchaseRepository : TrackingRepository<Purchase>, ITrackingRepository<Purchase>
    {
        public PurchaseRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<Purchase>> Get<T>(
            Expression<Func<Purchase, bool>> predicate,
            Expression<Func<Purchase, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            predicate = predicate.And(
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                    && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue));

            var res = new PagedList<Purchase>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                    await Context.Purchases
                        .AsNoTracking()
                        .Where(predicate)
                        .CountAsync()
            };

            res.AddRange(await Context.Purchases
                .AsNoTracking()
                .Include(s => s.Cart)
                .Include(s => s.Supplier)
                .Include(s => s.Metadata)
                .Where(predicate)
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<Purchase> GetById(int id)
            => await Context.Purchases
            .Include(s => s.Supplier)
            .Include(s => s.Cart)
            .Include(d => d.Metadata)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
