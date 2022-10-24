using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class VoucherRepository : TrackingRepository<Vouchar>
{
    public VoucherRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }

    public override async Task<IPagedList<Vouchar>> Get<T>(
            Expression<Func<Vouchar, bool>> predicate,
            Expression<Func<Vouchar, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<Vouchar>()
            .AsNoTracking()
            .Where(
                predicate.And(i => 
                    i.IsRemoved == false
                    && (from == null || i.Metadata.CreationTime >= from)
                    && (to == null || i.Metadata.CreationTime <= to)
                )
            )
            .Include(s => s.Supplier)
            .ToPagedListAsync(pageIndex, pageSize);        
    
    public override async Task<Vouchar?> GetById(int id) => 
        await Context.Set<Vouchar>()
            .Include(s => s.Supplier)
            .FirstOrDefaultAsync(s => s.Id == id);
}