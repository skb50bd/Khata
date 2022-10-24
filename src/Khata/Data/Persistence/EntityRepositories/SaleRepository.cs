using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;
using Throw;

namespace Data.Persistence.Repositories;

public class SaleRepository : TrackingRepository<Sale>, ISaleRepository
{
    public SaleRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime)
    { }

    public override async Task<IPagedList<Sale>> Get<T>(
            Expression<Func<Sale, bool>> predicate,
            Expression<Func<Sale, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<Sale>()
            .AsNoTracking()
            .OrderByDescending(order)
            .Where(predicate.AddTrackedDocumentFilter(from, to))
            .Include(s => s.Cart)
            .Include(s => s.Customer)
            .Include(s => s.Outlet)
            .ToPagedListAsync(pageIndex, pageSize);
    
    public override async Task<Sale?> GetById(int id) => 
        await Context.Set<Sale>()
            .Include(s => s.Customer)
            .Include(s => s.Cart)
            .Include(s => s.Outlet)
            .FirstOrDefaultAsync(s => s.Id == id);

    public async Task Save(SavedSale model)
    {
         await Context.Set<SavedSale>().AddAsync(model);
    }

    public async Task<IEnumerable<SavedSale>> GetSavedSales()
        => await Context.Set<SavedSale>()
            .AsNoTracking()
            .Include(s => s.Cart)
            .ToListAsync();

    public async Task<SavedSale?> GetSavedSale(int id) => 
        await Context.Set<SavedSale>()
            .AsNoTracking()
            .Include(s => s.Cart)
            .FirstOrDefaultAsync(ss => ss.Id == id);

    public async Task DeleteSaved(int id)
    {
        var item = await GetSavedSale(id);
        item.ThrowIfNull();
        Context.Entry(item).State = EntityState.Deleted;
        await Context.SaveChangesAsync();
    }

    public async Task DeleteAllSaved()
    {
        foreach (var item in await GetSavedSales())
        {
            Context.Entry(item).State = EntityState.Deleted;
        }
        
        await Context.SaveChangesAsync();

    }
}