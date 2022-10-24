using System.Linq.Expressions;
using Data.Core;

using Domain;
using Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : Document
{
    protected readonly KhataContext Context;
    protected readonly IDateTimeProvider _dateTime;
    
    public Repository(
        KhataContext context, 
        IDateTimeProvider dateTime)
    {
        Context = context;
        _dateTime = dateTime;
    }

    public virtual async Task<IPagedList<T>> Get<T2>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, T2>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null) =>
        await Context.Set<T>()
            .Where(predicate.AddDocumentFilter(from, to))
            .OrderByDescending(order)
            .AsNoTracking()
            .ToPagedListAsync(pageIndex, pageSize);

    public async Task<IPagedList<T>> Get(
            Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderFunc, 
            Func<IQueryable<T>, IQueryable<T>> includeFunc, 
            int pageIndex, 
            int pageSize, 
            DateTime? from = null,
            DateTime? to = null) =>
        await includeFunc(
            orderFunc(
                Context.Set<T>()
                    .Where(predicate.AddDocumentFilter(from, to))
            )
        ).ToPagedListAsync(pageSize, pageIndex);
        
    public virtual async Task<IList<T>> GetAll() => 
        await Context.Set<T>()
            .AsNoTracking()
            .ToListAsync();

    public virtual Task<T?> GetById(int id) => 
        Context.Set<T>()
            .FirstOrDefaultAsync(t => t.Id == id);

    public virtual async Task Add(T item, bool saveChanges = true)
    {
        await Context.AddAsync(item);

        if (saveChanges)
        {
            await Context.SaveChangesAsync();
        }
    }

    public virtual async Task AddAll(IEnumerable<T> items, bool saveChanges = true)
    {
        await Context.AddRangeAsync(items);

        if (saveChanges)
        {
            await Context.SaveChangesAsync();
        }
    }

    public virtual Task<bool> Exists(int id) => 
        Context.Set<T>().AnyAsync(e => e.Id == id);

    public virtual async Task Delete(int id, bool saveChanges = true)
    {
        var item = await GetById(id);
        Context.Remove(item);

        if (saveChanges)
        {
            await Context.SaveChangesAsync();
        }
    }

    public async Task<int> Count(DateTimeOffset? maybeFromDate = null, DateTimeOffset? maybeToDate = null) => 
        await Context.Set<T>()
            .AsNoTracking()
            .Where(document => 
                (maybeFromDate == null || document.Metadata.CreationTime >= maybeFromDate)
                && (maybeToDate == null || document.Metadata.CreationTime <= maybeFromDate)
            ).CountAsync();
}