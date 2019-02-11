using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Queries;

namespace Khata.Services.Reports
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