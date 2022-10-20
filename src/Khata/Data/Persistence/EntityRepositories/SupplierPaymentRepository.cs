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

public class SupplierPaymentRepository : TrackingRepository<SupplierPayment>, ITrackingRepository<SupplierPayment>
{
    public SupplierPaymentRepository(KhataContext context) : base(context) { }

    public override async Task<IPagedList<SupplierPayment>> Get<T>(
        Expression<Func<SupplierPayment, bool>> predicate,
        Expression<Func<SupplierPayment, T>> order,
        int pageIndex,
        int pageSize,
        DateTime? from = null,
        DateTime? to = null)
    {

        predicate = predicate.And(i => !i.IsRemoved
                                       && i.Metadata.CreationTime >= (from ?? Clock.Min)
                                       && i.Metadata.CreationTime <= (to ?? Clock.Max));
        var res = new PagedList<SupplierPayment>()
        {
            PageIndex = pageIndex,
            PageSize = pageSize,
            ResultCount =
                await Context.SupplierPayments
                    .AsNoTracking()
                    .Where(predicate)
                    .CountAsync()
        };

        res.AddRange(await Context.SupplierPayments
            .AsNoTracking()
            .Include(s => s.Supplier)
            .Where(predicate)
            .OrderByDescending(order)
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