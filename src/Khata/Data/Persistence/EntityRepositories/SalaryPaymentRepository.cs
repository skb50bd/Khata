using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class SalaryPaymentRepository : TrackingRepository<SalaryPayment>
{
    public SalaryPaymentRepository(
            KhataContext context,
            IDateTimeProvider dateTimeProvider) 
        : base(context, dateTimeProvider)
    { }

    public override async Task<IPagedList<SalaryPayment>> Get<T>(
            Expression<Func<SalaryPayment, bool>> predicate,
            Expression<Func<SalaryPayment, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) => 
        await Context.Set<SalaryPayment>()
            .AsNoTracking()
            .OrderByDescending(order)
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .Include(s => s.Employee)
            .ToPagedListAsync(pageIndex, pageSize);        
    
    public override async Task<SalaryPayment?> GetById(int id)
        => await Context.Set<SalaryPayment>()
            .Include(s => s.Employee)
            .FirstOrDefaultAsync(s => s.Id == id);
}