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
            Expression<Func<Sale, bool>> predicate,
            Expression<Func<Sale, T>> order,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<Sale, bool>> newPredicate =
                i => !i.IsRemoved && predicate.Compile().Invoke(i);

            var res = new PagedList<Sale>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.Sales.AsNoTracking().Where(predicate).CountAsync()
            };

            res.AddRange(await Context.Sales
                .AsNoTracking()
                .Include(s => s.Cart)
                .Include(s => s.Customer)
                .Where(newPredicate)
                .OrderBy(order)
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
    }
}
