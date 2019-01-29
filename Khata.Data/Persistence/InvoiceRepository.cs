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
    public class InvoiceRepository : TrackingRepository<CustomerInvoice>, ITrackingRepository<CustomerInvoice>
    {
        public InvoiceRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<CustomerInvoice>> Get<T>(
            Expression<Func<CustomerInvoice, bool>> predicate,
            Expression<Func<CustomerInvoice, T>> order,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<CustomerInvoice, bool>> newPredicate =
                i => !i.IsRemoved && predicate.Compile().Invoke(i);

            var res = new PagedList<CustomerInvoice>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.Invoices.AsNoTracking().Where(predicate).CountAsync()
            };

            res.AddRange(await Context.Invoices
                .AsNoTracking()
                .Include(s => s.Customer)
                .Include(s => s.Metadata)
                .Where(newPredicate)
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<CustomerInvoice> GetById(int id)
            => await Context.Invoices
            .Include(s => s.Customer)
            .Include(d => d.Metadata)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
