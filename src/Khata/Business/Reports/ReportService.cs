using System.Collections.Generic;
using System.Threading.Tasks;

using Data.Core;

using Domain.Reports;

namespace Business.Reports;

public class ReportService<TReport> 
    : IReportService<TReport> where TReport: Report
{
    private readonly IReportRepository<TReport> _repo;
    public ReportService(IReportRepository<TReport> repo) => 
        _repo = repo;

    public virtual async Task<TReport> Get() => 
        await _repo.Get();
}

public class ListReportService<TReport> 
    : IListReportService<TReport> where TReport:Report
{
    private readonly IListReportRepository<TReport> _repo;
    public ListReportService(
        IListReportRepository<TReport> repo) =>
        _repo = repo;

    public async Task<IEnumerable<TReport>> Get() => 
        await _repo.Get();

}

public class IndividualReportService<TReport> 
    : IIndividualReportService<TReport>
    where TReport : IndividualReport
{
    private readonly IIndividualReportRepository<TReport> _repo;
    public IndividualReportService(
        IIndividualReportRepository<TReport> repo) =>
        _repo = repo;

    public async Task<TReport> Get(int id)
        => await _repo.GetById(id);
}