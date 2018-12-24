using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Khata.Domain;

using Microsoft.EntityFrameworkCore;

namespace Khata.Data
{
    /// <summary>
    /// This is a repository and UOW on top of entity framework.
    /// It takes away a lot of the flexibility of entity framework, 
    /// but hides the soft delete in the database from the code, which may be desirable.
    /// This pattern makes more sense if there are other db fields of a tracking nature 
    /// like last updated time and user on a lot of different entities.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TrackingRepository<T> : ITrackingRepository<T> where T : TrackedEntity
    {
        protected KhataContext Context;
        public TrackingRepository(KhataContext context)
        {
            Context = context;
        }

        public virtual void Add(T item)
        {
            Context.Add(item);
        }

        public virtual void AddAll(IEnumerable<T> items)
        {
            Context.AddRange(items);
        }

        public virtual async Task Delete(int id)
        {
            var item = await GetById(id);
            item.Deleted = true;
        }

        public virtual IQueryable<T> Get()
        {
            return Context.Set<T>().Where(e => !e.Deleted);
        }

        public virtual async Task<List<T>> GetAll()
        {
            return await Context.Set<T>().Where(e => !e.Deleted).ToListAsync();
        }

        public virtual async Task<T> GetById(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public virtual void Save(T item)
        {
            Context.Update(item);
        }

        public virtual void SaveAll(IEnumerable<T> items)
        {
            Context.UpdateRange(items);
        }

        public virtual Task SaveChanges()
        {
            return Context.SaveChangesAsync();
        }
    }
}