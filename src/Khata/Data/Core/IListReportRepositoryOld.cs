using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Reports;

namespace Data.Core;

public interface IListReportRepositoryOld<TReport> where TReport : Report
{
    //Task<int> Count();
    Task<IEnumerable<TReport>> Get();
}