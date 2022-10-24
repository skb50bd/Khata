using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class InvoiceRepository : TrackingRepository<CustomerInvoice>, ITrackingRepository<CustomerInvoice>
{
    public InvoiceRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }

    public override async Task<IPagedList<CustomerInvoice>> Get<T>(
            Expression<Func<CustomerInvoice, bool>> predicate,
            Expression<Func<CustomerInvoice, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<CustomerInvoice>()
            .AsNoTracking()
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .Include(d => d.Customer)
            .Include(d => d.Outlet)
            .OrderByDescending(order)
            .ToPagedListAsync(pageIndex, pageSize);

    public override async Task<CustomerInvoice?> GetById(int id) => 
        await Context.Set<CustomerInvoice>()
            .Include(s => s.Customer)
            .Include(d => d.Outlet)
            .FirstOrDefaultAsync(s => s.Id == id);
}