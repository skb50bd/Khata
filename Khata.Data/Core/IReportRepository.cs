using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Queries;

namespace Khata.Data.Core
{
    public interface IReportRepository<TReport> where TReport : Report
    {
        Task<int> Count();
        Task<IEnumerable<TReport>> Get();
    }

    public interface IIndividualReportRepository<TReport> where TReport : IndividaulReport
    {
        Task<TReport> GetById(int id);
    }
}