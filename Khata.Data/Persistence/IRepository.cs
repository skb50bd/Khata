using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data
{
    public interface IRepository<T> where T : Entity
    {
        Task<List<T>> GetAll();
        Task<T> GetById(int id);
        Task<List<T>> Get<T2>(Expression<Func<T, bool>> predicate, Expression<Func<T, T2>> order);
        Task Save(T item);
        Task Add(T item);
        Task SaveAll(IEnumerable<T> items);
        Task AddAll(IEnumerable<T> items);
        Task Delete(int primaryKey);
    }
}
