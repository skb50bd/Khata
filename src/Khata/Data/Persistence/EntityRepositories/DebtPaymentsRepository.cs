using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class DebtPaymentRepository : TrackingRepository<DebtPayment>, ITrackingRepository<DebtPayment>
{
    private readonly IDateTimeProvider _dateTime;
    
    public DebtPaymentRepository(
        KhataContext context, 
        IDateTimeProvider dateTime
    ) : base(context, dateTime)
    {
        _dateTime = dateTime;
    }

    public override async Task<IPagedList<DebtPayment>> Get<T>(
            Expression<Func<DebtPayment, bool>> predicate,
            Expression<Func<DebtPayment, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context
            .Set<DebtPayment>()
            .AsNoTracking()
            .Include(_ => _.Customer)
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .OrderByDescending(order)
            .ToPagedListAsync(pageIndex, pageSize);

    public override async Task<DebtPayment?> GetById(int id) => 
        await Context.Set<DebtPayment>()
            .Include(s => s.Customer)
            .FirstOrDefaultAsync(s => s.Id == id);
}