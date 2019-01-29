using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Khata.Domain;

using SharedLibrary;

namespace Khata.Data.Core
{
    public interface IRepository<T> where T : Document
    {
        Task<IList<T>> GetAll();
        Task<T> GetById(int id);
        Task<IPagedList<T>> Get<T2>(Expression<Func<T, bool>> predicate, Expression<Func<T, T2>> order, int pageIndex, int pageSize);
        void Add(T item);
        void AddAll(IEnumerable<T> items);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task<int> Count();
    }
}
