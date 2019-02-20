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
                    && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                    && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue));

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
                .Include(s => s.Metadata)
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
                .Include(d => d.Metadata)
                .FirstOrDefaultAsync(s => s.Id == id);
    }
}
