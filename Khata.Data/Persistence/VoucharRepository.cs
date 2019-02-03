﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using Microsoft.EntityFrameworkCore;

using SharedLibrary;

namespace Khata.Data.Persistence
{
    public class VoucharRepository : TrackingRepository<Vouchar>, ITrackingRepository<Vouchar>
    {
        public VoucharRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<Vouchar>> Get<T>(
            Predicate<Vouchar> predicate,
            Expression<Func<Vouchar, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            Predicate<Vouchar> newPredicate =
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                    && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue)
                    && predicate(i);

            var res = new PagedList<Vouchar>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                    await Context.Vouchars
                        .AsNoTracking()
                        .Include(e => e.Metadata)
                        .Where(s => newPredicate(s))
                        .CountAsync()
            };

            res.AddRange(await Context.Vouchars
                .AsNoTracking()
                .Include(s => s.Supplier)
                .Include(s => s.Metadata)
                .Where(s => newPredicate(s))
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<Vouchar> GetById(int id)
            => await Context.Vouchars
            .Include(s => s.Supplier)
            .Include(d => d.Metadata)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
