using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data
{
    public interface ITrackingRepository<T> where T : TrackedEntity
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        IQueryable<T> Get();
        void Save(T item);
        void Add(T item);
        void SaveAll(IEnumerable<T> items);
        void AddAll(IEnumerable<T> items);
        Task Delete(int primaryKey);

        Task SaveChanges();

    }
}
