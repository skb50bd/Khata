using System.Linq.Expressions;
using Domain;

namespace Data.Core;

public interface IRepository<T> where T : Document
{
    Task<IList<T>> GetAll();
    
    Task<T?> GetById(int id);
    
    Task<IPagedList<T>> Get<T2>(
        Expression<Func<T, bool>> predicate,
        Expression<Func<T, T2>> order,
        int pageIndex,
        int pageSize,
        DateTime? from = null,
        DateTime? to = null);
    
    Task<IPagedList<T>> Get(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IOrderedQueryable<T>> orderFunc,
        Func<IQueryable<T>, IQueryable<T>> includes,
        int pageIndex,
        int pageSize,
        DateTime? from = null,
        DateTime? to = null);
    
    Task Add(T item, bool saveChanges = true);
    
    Task AddAll(IEnumerable<T> items, bool saveChanges = true);
    
    Task Delete(int id, bool saveChanges = true);
    
    Task<bool> Exists(int id);
    
    Task<int> Count(DateTimeOffset? from = null, DateTimeOffset? to = null);
}