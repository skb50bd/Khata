using System.Collections.Generic;
using System.Threading.Tasks;

using Queries;

namespace Business.Reports
{
    public interface IReportService<TReport> where TReport : Report
    {
        Task<int> Count();
        Task<IEnumerable<TReport>> Get();
    }

    public interface IIndividualReportService<TReport> : IReportService<TReport>
        where TReport : IndividaulReport
    {
        Task<TReport> Get(int id);
    }
}