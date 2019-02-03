using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using Microsoft.EntityFrameworkCore;

using SharedLibrary;

namespace Khata.Data.Persistence
{
    public class SaleRepository : TrackingRepository<Sale>, ITrackingRepository<Sale>
    {
        public SaleRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<Sale>> Get<T>(
            Predicate<Sale> predicate,
            Expression<Func<Sale, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            Predicate<Sale> newPredicate =
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                    && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue)
                    && predicate(i);

            var res = new PagedList<Sale>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                    await Context.Sales
                        .AsNoTracking()
                        .Include(d => d.Metadata)
                        .Where(i => !i.IsRemoved
                                && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                                && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue)
                                && predicate(i))
                        .CountAsync()
            };

            res.AddRange(await Context.Sales
                .AsNoTracking()
                .Include(s => s.Cart)
                .Include(s => s.Customer)
                .Include(s => s.Metadata)
                .Where(i => !i.IsRemoved
                            && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                            && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue)
                            && predicate(i))
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<Sale> GetById(int id)
            => await Context.Sales
            .Include(s => s.Customer)
            .Include(s => s.Cart)
            .Include(d => d.Metadata)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
