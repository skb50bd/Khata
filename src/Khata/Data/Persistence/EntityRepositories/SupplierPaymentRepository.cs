using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class SupplierPaymentRepository : TrackingRepository<SupplierPayment>
{
    public SupplierPaymentRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }

    public override async Task<IPagedList<SupplierPayment>> Get<T>(
            Expression<Func<SupplierPayment, bool>> predicate,
            Expression<Func<SupplierPayment, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<SupplierPayment>()
            .AsNoTracking()
            .OrderByDescending(order)
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .Include(s => s.Supplier)
            .ToPagedListAsync(pageIndex, pageSize);

    public override async Task<SupplierPayment?> GetById(int id) => 
        await Context.Set<SupplierPayment>()
            .Include(s => s.Supplier)
            .FirstOrDefaultAsync(s => s.Id == id);
}