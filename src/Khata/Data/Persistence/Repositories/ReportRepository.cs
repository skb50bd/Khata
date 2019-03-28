using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Data.Core;

using Microsoft.EntityFrameworkCore;

using Queries;

namespace Data.Persistence.Repositories
{
    public class ReportRepository<TReport> 
        : IReportRepository<TReport> where TReport : Report
    {
        protected readonly KhataContext Db;
        public ReportRepository(KhataContext db) 
            => Db = db;

        public async Task<int> Count()
            => (await Db.Query<TReport>()
                .ToListAsync()).Count();

        public async Task<IEnumerable<TReport>> Get()
            => await Db.Query<TReport>()
                .ToListAsync();

    }

    public class IndividualReportRepository<TReport>
        : ReportRepository<TReport>,
            IIndividualReportRepository<TReport>
                where TReport : IndividaulReport
    {
        public IndividualReportRepository(
            KhataContext db) : base(db) { }

        public async Task<TReport> GetById(int id)
            => await Db.Query<TReport>()
                .FirstOrDefaultAsync(
                    r => r.Id == id);
    }
}
