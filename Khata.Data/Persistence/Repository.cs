using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using Microsoft.EntityFrameworkCore;

using SharedLibrary;

namespace Khata.Data.Persistence
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        protected readonly KhataContext Context;
        public Repository(KhataContext context)
        {
            Context = context;
        }

        public virtual async Task<IPagedList<T>> Get<T2>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, T2>> order,
            int pageIndex,
            int pageSize)
        {
            var res = new PagedList<T>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount = await Context.Set<T>().AsNoTracking().Where(predicate).CountAsync()
            };

            res.AddRange(await Context.Set<T>()
                                      .AsNoTracking()
                                      .Where(predicate)
                                      .OrderBy(order)
                                      .Skip((pageIndex - 1) * pageSize)
                                      .Take(pageSize > 0 ? pageSize : int.MaxValue)
                                      .ToListAsync());
            return res;
        }

        public virtual async Task<IList<T>> GetAll()
            => await Context.Set<T>()
                        .AsNoTracking()
                        .ToListAsync();

        public virtual async Task<T> GetById(int id)
            => await Context.Set<T>()
                        .FindAsync(id);

        public virtual void Add(T item)
            => Context.Add(item);

        public virtual void AddAll(IEnumerable<T> items)
            => Context.AddRange(items);

        public virtual async Task<bool> Exists(int id)
            => await Context.Set<T>()
                        .AnyAsync(e => e.Id == id);

        public virtual async Task Delete(int id)
            => Context.Remove(await GetById(id));

        public async Task<int> Count()
            => await Context.Set<T>().CountAsync();
    }
}