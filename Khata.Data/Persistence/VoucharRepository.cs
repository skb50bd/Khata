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
    public class VoucharRepository : TrackingRepository<Vouchar>, ITrackingRepository<Vouchar>
    {
        public VoucharRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<Vouchar>> Get<T>(
            Expression<Func<Vouchar, bool>> predicate,
            Expression<Func<Vouchar, T>> order,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<Vouchar, bool>> newPredicate =
                i => !i.IsRemoved && predicate.Compile().Invoke(i);

            var res = new PagedList<Vouchar>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.Vouchars.AsNoTracking().Where(predicate).CountAsync()
            };

            res.AddRange(await Context.Vouchars
                .AsNoTracking()
                .Include(s => s.Supplier)
                .Where(newPredicate)
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<Vouchar> GetById(int id)
            => await Context.Vouchars
            .Include(s => s.Supplier)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
