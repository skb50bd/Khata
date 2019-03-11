using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Brotal.Extensions;

using Data.Core;
using Domain;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public class DebtPaymentRepository : TrackingRepository<DebtPayment>, ITrackingRepository<DebtPayment>
    {
        public DebtPaymentRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<DebtPayment>> Get<T>(
            Expression<Func<DebtPayment, bool>> predicate,
            Expression<Func<DebtPayment, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {

            predicate = predicate.And(i => !i.IsRemoved
                            && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                            && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue));

            var res = new PagedList<DebtPayment>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.DebtPayments
                    .AsNoTracking()
                    .Include(d => d.Customer)
                    .Where(predicate)
                    .CountAsync()
            };

            res.AddRange(await Context.DebtPayments
                .AsNoTracking()
                .Include(s => s.Customer)
                .Where(predicate)
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<DebtPayment> GetById(int id)
            => await Context.DebtPayments
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
