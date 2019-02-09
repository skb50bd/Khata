using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Queries;

namespace Khata.Services.Reports
{
    public class ReportService<TReport> : IReportService<TReport> where TReport : Report
    {
        private readonly IReportRepository<TReport> _reports;
        public ReportService(IReportRepository<TReport> reports)
        {
            _reports = reports;
        }

        public async Task<IEnumerable<TReport>> Get()
            => await _reports.Get();

        public async Task<TReport> Get(int id)
            => await _reports.GetById(id);

        public async Task<int> Count()
            => await _reports.Count();
    }
}
