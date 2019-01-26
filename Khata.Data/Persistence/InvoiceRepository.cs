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
    public class InvoiceRepository : TrackingRepository<Invoice>, ITrackingRepository<Invoice>
    {
        public InvoiceRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<Invoice>> Get<T>(
            Expression<Func<Invoice, bool>> predicate,
            Expression<Func<Invoice, T>> order,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<Invoice, bool>> newPredicate =
                i => !i.IsRemoved && predicate.Compile().Invoke(i);

            var res = new PagedList<Invoice>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.Invoices.AsNoTracking().Where(predicate).CountAsync()
            };

            res.AddRange(await Context.Invoices
                .AsNoTracking()
                .Include(s => s.Customer)
                .Where(newPredicate)
                .OrderBy(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<Invoice> GetById(int id)
            => await Context.Invoices
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
