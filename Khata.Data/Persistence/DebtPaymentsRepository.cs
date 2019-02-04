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
    public class DebtPaymentRepository : TrackingRepository<DebtPayment>, ITrackingRepository<DebtPayment>
    {
        public DebtPaymentRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<DebtPayment>> Get<T>(
            Predicate<DebtPayment> predicate,
            Expression<Func<DebtPayment, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            var res = new PagedList<DebtPayment>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.DebtPayments
                    .AsNoTracking()
                    .Include(d => d.Metadata)
                    .Where(i => !i.IsRemoved
                            && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                            && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue)
                            && predicate(i))
                    .CountAsync()
            };

            res.AddRange(await Context.DebtPayments
                .AsNoTracking()
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

        public override async Task<DebtPayment> GetById(int id)
            => await Context.DebtPayments
            .Include(s => s.Customer)
            .Include(d => d.Metadata)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
