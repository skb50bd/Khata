using System.Collections.Generic;
using System.Threading.Tasks;

using Business.Reports;

using Domain.Reports;

using Microsoft.AspNetCore.SignalR;


namespace WebUI.Hubs;

public class ReportsHub : Hub
{
    #region Dependencies
    private readonly IReportService<Asset> _assetReport;
    private readonly IReportService<Liability> _liabilityReport;
    private readonly IReportService<PeriodicalReport<Inflow>> _inflowReport;
    private readonly IReportService<PeriodicalReport<Outflow>> _outflowReport;
    private readonly IReportService<PeriodicalReport<Payable>> _payableReport;
    private readonly IReportService<PeriodicalReport<Receivable>> _receivableReport;
    private readonly IListReportService<PerDayReport> _perDayReports;
    #endregion

    #region Data Properties
    public Asset Asset { get; set; }
    public Liability Liability { get; set; }
    public PeriodicalReport<Inflow> Inflow { get; set; }
    public PeriodicalReport<Outflow> Outflow { get; set; }
    public PeriodicalReport<Payable> Payable { get; set; }
    public PeriodicalReport<Receivable> Receivable { get; set; }
    public IEnumerable<PerDayReport> PerDayReports { get; set; }
    #endregion

    public ReportsHub(
        #region Injected Dependencies
        IReportService<Asset> assetReport,
        IReportService<Liability> liabilityReport,
        IReportService<PeriodicalReport<Inflow>> inflowReport,
        IReportService<PeriodicalReport<Outflow>> outflowReport,
        IReportService<PeriodicalReport<Payable>> payableReport,
        IReportService<PeriodicalReport<Receivable>> receivableReport,
        IListReportService<PerDayReport> perDayReports 
        #endregion
    )
    {
        _assetReport = assetReport;
        _liabilityReport = liabilityReport;
        _inflowReport = inflowReport;
        _outflowReport = outflowReport;
        _payableReport = payableReport;
        _receivableReport = receivableReport;
        _perDayReports = perDayReports;
    }

    public async Task UpdateChartData()
    {
        Asset         = await _assetReport.Get();
        Liability     = await _liabilityReport.Get();
        Inflow        = await _inflowReport.Get();
        Outflow       = await _outflowReport.Get();
        Payable       = await _payableReport.Get();
        Receivable    = await _receivableReport.Get();
        PerDayReports = await _perDayReports.Get();
    }

    public async Task InitChartData()
    {
        await UpdateChartData();
        await Clients.All.SendAsync(
            "UpdateChart",
            new
            {
                Asset,
                Liability,
                Income = Inflow,
                Expense = Outflow,
                Payable,
                Receivable,
                PerDayReports
            });
    }

    public async Task RefreshData() 
        => await InitChartData();
}