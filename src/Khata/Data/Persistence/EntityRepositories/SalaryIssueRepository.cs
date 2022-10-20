using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Brotal;
using Brotal.Extensions;

using Data.Core;

using Domain;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class SalaryIssueRepository : TrackingRepository<SalaryIssue>, ITrackingRepository<SalaryIssue>
{
    public SalaryIssueRepository(KhataContext context) : base(context) { }

    public override async Task<IPagedList<SalaryIssue>> Get<T>(
        Expression<Func<SalaryIssue, bool>> predicate,
        Expression<Func<SalaryIssue, T>> order,
        int pageIndex,
        int pageSize,
        DateTime? from = null,
        DateTime? to = null)
    {
        predicate = predicate.And(
            i => !i.IsRemoved
                 && i.Metadata.CreationTime >= (from ?? Clock.Min)
                 && i.Metadata.CreationTime <= (to ?? Clock.Max));

        var res = new PagedList<SalaryIssue>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            ResultCount =
                await Context.SalaryIssues
                    .AsNoTracking()
                    .Where(predicate)
                    .CountAsync()
        };

        res.AddRange(await Context.SalaryIssues
            .AsNoTracking()
            .Include(s => s.Employee)
            .Where(predicate)
            .OrderByDescending(order)
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