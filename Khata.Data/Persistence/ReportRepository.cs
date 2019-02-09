using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Queries;

using Microsoft.EntityFrameworkCore;

namespace Khata.Data.Persistence
{
    public class ReportRepository<TReport>
        : IReportRepository<TReport> where TReport : Report
    {
        private readonly KhataContext _db;
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

        public async Task<TReport> GetById(int id)
            => await _db.Query<TReport>()
            .FirstOrDefaultAsync(r => r.Id == id);
    }
}
