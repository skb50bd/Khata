﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Brotal;

using Domain;

namespace Data.Core
{
    public interface IRepository<T> where T : Document
    {
        Task<IList<T>> GetAll();
        Task<T> GetById(int id);
        Task<IPagedList<T>> Get<T2>(
            Expression<Func<T, bool>> predicate,
            Expression<Func<T, T2>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null);
        void Add(T item);
        void AddAll(IEnumerable<T> items);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task<int> Count(DateTime? from = null, DateTime? to = null);
    }
}
