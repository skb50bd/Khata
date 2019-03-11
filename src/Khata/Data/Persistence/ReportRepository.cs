using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Data.Core;
using Queries;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence
{
    public class ReportRepository<TReport> : IReportRepository<TReport> where TReport : Report
    {
        protected readonly KhataContext _db;
        public ReportRepository(KhataContext db)
        {
            _db = db;
        }
        public async Task<int> Count()
            => (await _db.Query<TReport>()
            .ToListAsync()).Count();

        public async Task<IEnumerable<TReport>> Get()
            => await _db.Query<TReport>()
            .ToListAsync();

    }

    public class IndividualReportRepository<TReport>
        : ReportRepository<TReport>,
            IIndividualReportRepository<TReport>
                where TReport : IndividaulReport
    {
        public IndividualReportRepository(KhataContext db) : base(db) { }

        public async Task<TReport> GetById(int id)
            => await _db.Query<TReport>()
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
