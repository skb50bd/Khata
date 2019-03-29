using System.Collections.Generic;
using System.Threading.Tasks;

using Domain.Reports;

namespace Business.Reports
{
    public interface IReportService<TReport> 
        where TReport : Report
    {
        Task<TReport> Get();
    }

    public interface IListReportService<TReport> 
        where TReport : Report
    {
        Task<IEnumerable<TReport>> Get();
    }

    public interface IIndividualReportService<TReport>
        where TReport : IndividualReport
    {
        Task<TReport> Get(int id);
    }
}