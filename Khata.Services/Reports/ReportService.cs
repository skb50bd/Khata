using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Queries;

namespace Khata.Services.Reports
{
    public class ReportService<TReport>
        : IReportService<TReport>
            where TReport : Report
    {
        protected readonly IReportRepository<TReport> _reports;
        public ReportService(IReportRepository<TReport> reports)
        {
            _reports = reports;
        }

        public async Task<IEnumerable<TReport>> Get()
            => await _reports.Get();


        public async Task<int> Count()
            => await _reports.Count();
    }

    public class IndividualReportService<TReport> :
        ReportService<TReport>, IIndividualReportService<TReport>
            where TReport : IndividaulReport
    {
        protected readonly IIndividualReportRepository<TReport> _individualReports;
        public IndividualReportService(IIndividualReportRepository<TReport> reports)
            : base(reports as IReportRepository<TReport>)
        {
            _individualReports = reports;
        }

        public async Task<TReport> Get(int id)
            => await _individualReports.GetById(id);
    }
}
