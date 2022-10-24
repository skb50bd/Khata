using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class SalaryIssueRepository : TrackingRepository<SalaryIssue>, ITrackingRepository<SalaryIssue>
{
    public SalaryIssueRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }

    public override async Task<IPagedList<SalaryIssue>> Get<T>(
            Expression<Func<SalaryIssue, bool>> predicate,
            Expression<Func<SalaryIssue, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<SalaryIssue>()
            .AsNoTracking()
            .OrderByDescending(order)
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .Include(s => s.Employee)
            .ToPagedListAsync(pageIndex, pageSize);

    public override async Task<SalaryIssue?> GetById(int id)
        => await Context.Set<SalaryIssue>()
            .Include(s => s.Employee)
            .FirstOrDefaultAsync(s => s.Id == id);
}