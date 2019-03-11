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
    public class SalaryPaymentRepository : TrackingRepository<SalaryPayment>, ITrackingRepository<SalaryPayment>
    {
        public SalaryPaymentRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<SalaryPayment>> Get<T>(
            Expression<Func<SalaryPayment, bool>> predicate,
            Expression<Func<SalaryPayment, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            predicate = predicate.And(
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                    && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue));

            var res = new PagedList<SalaryPayment>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                await Context.SalaryPayments
                    .AsNoTracking()
                    .Where(predicate)
                    .CountAsync()
            };

            res.AddRange(await Context.SalaryPayments
                .AsNoTracking()
                .Include(s => s.Employee)
                .Where(predicate)
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<SalaryPayment> GetById(int id)
            => await Context.SalaryPayments
            .Include(s => s.Employee)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
