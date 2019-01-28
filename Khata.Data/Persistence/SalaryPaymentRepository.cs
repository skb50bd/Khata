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
    public class SalaryPaymentRepository : TrackingRepository<SalaryPayment>, ITrackingRepository<SalaryPayment>
    {
        public SalaryPaymentRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<SalaryPayment>> Get<T>(
            Expression<Func<SalaryPayment, bool>> predicate,
            Expression<Func<SalaryPayment, T>> order,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<SalaryPayment, bool>> newPredicate =
                i => !i.IsRemoved && predicate.Compile().Invoke(i);

            var res = new PagedList<SalaryPayment>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.SalaryPayments.AsNoTracking().Where(predicate).CountAsync()
            };

            res.AddRange(await Context.SalaryPayments
                .AsNoTracking()
                .Include(s => s.Employee)
                .Where(newPredicate)
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
