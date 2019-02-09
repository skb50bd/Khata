using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Queries;

namespace Khata.Data.Core
{
    public interface IReportRepository<TReport> where TReport : Report
    {
        Task<int> Count();
        Task<IEnumerable<TReport>> Get();
        Task<TReport> GetById(int id);
    }
}