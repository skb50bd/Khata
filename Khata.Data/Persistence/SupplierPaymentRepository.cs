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
    public class SupplierPaymentRepository : TrackingRepository<SupplierPayment>, ITrackingRepository<SupplierPayment>
    {
        public SupplierPaymentRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<SupplierPayment>> Get<T>(
            Expression<Func<SupplierPayment, bool>> predicate,
            Expression<Func<SupplierPayment, T>> order,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<SupplierPayment, bool>> newPredicate =
                i => !i.IsRemoved && predicate.Compile().Invoke(i);

            var res = new PagedList<SupplierPayment>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.SupplierPayments.AsNoTracking().Where(predicate).CountAsync()
            };

            res.AddRange(await Context.SupplierPayments
                .AsNoTracking()
                .Include(s => s.Supplier)
                .Where(newPredicate)
                .OrderBy(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<SupplierPayment> GetById(int id)
            => await Context.SupplierPayments
            .Include(s => s.Supplier)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
