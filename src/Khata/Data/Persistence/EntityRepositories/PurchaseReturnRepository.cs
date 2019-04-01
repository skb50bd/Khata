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
    public class PurchaseReturnRepository : TrackingRepository<PurchaseReturn>, ITrackingRepository<PurchaseReturn>
    {
        public PurchaseReturnRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<PurchaseReturn>> Get<T>(
            Expression<Func<PurchaseReturn, bool>> predicate,
            Expression<Func<PurchaseReturn, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            predicate = predicate.And(
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? Clock.Min)
                    && i.Metadata.CreationTime <= (to ?? Clock.Max));

            var res = new PagedList<PurchaseReturn>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                    await Context.PurchaseReturns
                        .AsNoTracking()
                        .Where(predicate)
                        .CountAsync()
            };

            res.AddRange(await Context.PurchaseReturns
                .AsNoTracking()
                .Include(s => s.Cart)
                .Include(s => s.Supplier)
                .Where(predicate)
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<PurchaseReturn> GetById(int id)
            => await Context.PurchaseReturns
                .Include(s => s.Supplier)
                .Include(s => s.Cart)
                .FirstOrDefaultAsync(s => s.Id == id);
    }
}
