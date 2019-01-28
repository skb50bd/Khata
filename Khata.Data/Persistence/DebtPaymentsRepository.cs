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
            Expression<Func<DebtPayment, bool>> predicate,
            Expression<Func<DebtPayment, T>> order,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<DebtPayment, bool>> newPredicate =
                i => !i.IsRemoved && predicate.Compile().Invoke(i);

            var res = new PagedList<DebtPayment>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.DebtPayments.AsNoTracking().Where(predicate).CountAsync()
            };

            res.AddRange(await Context.DebtPayments
                .AsNoTracking()
                .Include(s => s.Customer)
                .Where(newPredicate)
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
