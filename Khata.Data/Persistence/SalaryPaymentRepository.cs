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
    public class SalaryPaymentRepository : TrackingRepository<SalaryPayment>, ITrackingRepository<SalaryPayment>
    {
        public SalaryPaymentRepository(KhataContext context) : base(context) { }

        public override async Task<IPagedList<SalaryPayment>> Get<T>(
            Predicate<SalaryPayment> predicate,
            Expression<Func<SalaryPayment, T>> order,
            int pageIndex,
            int pageSize,
            DateTime? from = null,
            DateTime? to = null)
        {
            Predicate<SalaryPayment> newPredicate =
                i => !i.IsRemoved
                    && i.Metadata.CreationTime >= (from ?? DateTime.MinValue)
                    && i.Metadata.CreationTime <= (to ?? DateTime.MaxValue)
                    && predicate(i);

            var res = new PagedList<SalaryPayment>()
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                ResultCount =
                await Context.SalaryPayments
                    .AsNoTracking()
                    .Include(d => d.Metadata)
                    .Where(s => newPredicate(s))
                    .CountAsync()
            };

            res.AddRange(await Context.SalaryPayments
                .AsNoTracking()
                .Include(s => s.Employee)
                .Include(s => s.Metadata)
                .Where(s => newPredicate(s))
                .OrderByDescending(order)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize > 0 ? pageSize : int.MaxValue)
                .ToListAsync());

            return res;
        }

        public override async Task<SalaryPayment> GetById(int id)
            => await Context.SalaryPayments
            .Include(s => s.Employee)
            .Include(d => d.Metadata)
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
