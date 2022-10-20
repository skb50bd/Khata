using System.Collections.Generic;
using System.Threading.Tasks;

using Domain.Reports;

namespace Data.Core;

public interface IReportRepository<TReport> where TReport : Report
{
    Task<TReport> Get();
}

public interface IListReportRepository<TReport> where TReport : Report
{
    Task<IEnumerable<TReport>> Get();
}

public interface IIndividualReportRepository<TReport> where TReport : IndividualReport
{
    Task<TReport> GetById(int id);
}