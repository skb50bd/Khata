using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using Microsoft.EntityFrameworkCore;

using Brotal.Extensions;

namespace Khata.Data.Persistence
{
    public class SaleRepository : TrackingRepository<Sale>, ISaleRepository
    {
        public SaleRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<Sale>> Get<T>(
            Expression<Func<Sale, bool>> predicate,
            Expression<Func<Sale, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            predicate = predicate.And(
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                    && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue));

            var res = new PagedList<Sale>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                    await Context.Sales
                        .AsNoTracking()
                        .Where(predicate)
                        .CountAsync()
            };

            res.AddRange(await Context.Sales
                .AsNoTracking()
                .Include(s => s.Cart)
                .Include(s => s.Customer)
                .Where(predicate)
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
                        .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<IEnumerable<Sale>> GetSavedSales()
            => await Context.SavedSales
                        .AsNoTracking()
                        .Include(s => s.Customer)
                        .Include(s => s.Cart)
                        .ToListAsync();

        public async Task<Sale> GetSavedSale(int id)
            => await Context.SavedSales
                        .AsNoTracking()
                        .Include(s => s.Customer)
                        .Include(s => s.Cart)
                        .FirstOrDefaultAsync(ss => ss.Id == id);

        public async Task DeleteSavedSale(int id)
        {
            var item = await GetSavedSale(id);
            Context.Entry(item).State = EntityState.Deleted;
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAllSavedSales()
        {
            foreach (var item in await GetSavedSales())
            {
                Context.Entry(item).State = EntityState.Deleted;
            }
            await Context.SaveChangesAsync();

        }
    }
}
