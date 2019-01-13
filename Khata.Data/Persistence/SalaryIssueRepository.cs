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
    public class SalaryIssueRepository : TrackingRepository<SalaryIssue>, ITrackingRepository<SalaryIssue>
    {
        public SalaryIssueRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<SalaryIssue>> Get<T>(
            Expression<Func<SalaryIssue, bool>> predicate,
            Expression<Func<SalaryIssue, T>> order,
            int pageIndex,
            int pageSize)
        {
            Expression<Func<SalaryIssue, bool>> newPredicate =
                i => !i.IsRemoved && predicate.Compile().Invoke(i);

            var res = new PagedList<SalaryIssue>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.SalaryIssues.AsNoTracking().Where(predicate).CountAsync()
            };

            res.AddRange(await Context.SalaryIssues
                .AsNoTracking()
                .Include(s => s.Employee)
                .Where(newPredicate)
                .OrderBy(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<SalaryIssue> GetById(int id)
            => await Context.SalaryIssues
            .Include(s => s.Employee)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
