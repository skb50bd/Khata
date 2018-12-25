using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using Microsoft.EntityFrameworkCore;

namespace Khata.Data.Persistence
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly KhataContext _context;
        public Repository(KhataContext context)
        {
            _context = context;
        }

        public virtual async Task<IList<T>> Get<T2>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, T2>> order,
            int pageIndex,
            int pageSize)
            => await _context.Set<T>()
                        .AsNoTracking()
                        .Where(predicate)
                        .OrderBy(order)
                        .Skip((pageIndex - 1) * pageSize)
                        .Take(pageSize > 0 ? pageSize : int.MaxValue)
                        .ToListAsync();

        public virtual async Task<IList<T>> GetAll()
            => await _context.Set<T>()
                        .AsNoTracking()
                        .ToListAsync();

        public virtual async Task<T> GetById(int id)
            => await _context.Set<T>()
                        .FindAsync(id);

        public virtual void Add(T item)
            => _context.Add(item);

        public virtual void AddAll(IEnumerable<T> items)
            => _context.AddRange(items);

        public virtual async Task<bool> Exists(int id)
            => await _context.Set<T>()
                        .AnyAsync(e => e.Id == id);

        public virtual async Task Delete(int id)
            => _context.Remove(await GetById(id));

        public async Task<int> Count()
            => await _context.Set<T>().CountAsync();
    }
}