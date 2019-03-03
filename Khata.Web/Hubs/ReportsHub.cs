using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Khata.Queries;
using Khata.Services.Reports;

using Microsoft.AspNetCore.SignalR;

namespace WebUI.Hubs
{
    public class ReportsHub : Hub
    {
        #region Dependencies
        private readonly IReportService<AssetReport> _assetReport;
        private readonly IReportService<LiabilityReport> _liabilityReport;
        private readonly IReportService<DailyIncomeReport> _dailyIncomeReports;
        private readonly IReportService<WeeklyIncomeReport> _weeklyIncomeReports;
        private readonly IReportService<MonthlyIncomeReport> _monthlyIncomeReports;
        private readonly IReportService<DailyExpenseReport> _dailyExpenseReports;
        private readonly IReportService<WeeklyExpenseReport> _weeklyExpenseReports;
        private readonly IReportService<MonthlyExpenseReport> _monthlyExpenseReports;
        private readonly IReportService<DailyPayableReport> _dailyPayableReports;
        private readonly IReportService<WeeklyPayableReport> _weeklyPayableReports;
        private readonly IReportService<MonthlyPayableReport> _monthlyPayableReports;
        private readonly IReportService<DailyReceivableReport> _dailyReceivableReports;
        private readonly IReportService<WeeklyReceivableReport> _weeklyReceivableReports;
        private readonly IReportService<MonthlyReceivableReport> _monthlyReceivableReports;
        private readonly IReportService<DailyOutletSalesReport> _dailyOutletSalesReports;
        private readonly IReportService<WeeklyOutletSalesReport> _weeklyOutletSalesReports;
        private readonly IReportService<MonthlyOutletSalesReport> _monthlyOutletSalesReports;
        private readonly IReportService<PerDayReport> _perDayReports;
        #endregion

        #region Data Properties (From QUERIES)
        public AssetReport Asset { get; set; }
        public LiabilityReport Liability { get; set; }
        public IncomeReports Income { get; set; }
        public ExpenseReports Expense { get; set; }
        public PayableReports Payable { get; set; }
        public ReceivableReports Receivable { get; set; }
        public OutletSalesReport OutletSales { get; set; }
        public IEnumerable<PerDayReport> PerDayReports { get; set; }
        #endregion

        public ReportsHub(
        #region Injected Dependencies
            IReportService<AssetReport> assetReport,
            IReportService<LiabilityReport> liabilityReport,
            IReportService<DailyIncomeReport> dailyIncomeReports,
            IReportService<WeeklyIncomeReport> weeklyIncomeReports,
            IReportService<MonthlyIncomeReport> monthlyIncomeReports,
            IReportService<DailyExpenseReport> dailyExpenseReports,
            IReportService<WeeklyExpenseReport> weeklyExpenseReports,
            IReportService<MonthlyExpenseReport> monthlyExpenseReports,
            IReportService<DailyPayableReport> dailyPayableReports,
            IReportService<WeeklyPayableReport> weeklyPayableReports,
            IReportService<MonthlyPayableReport> monthlyPayableReports,
            IReportService<DailyReceivableReport> dailyReceivableReports,
            IReportService<WeeklyReceivableReport> weeklyReceivableReports,
            IReportService<MonthlyReceivableReport> monthlyReceivableReports,
            IReportService<DailyOutletSalesReport> dailyOutletSalesReports,
            IReportService<WeeklyOutletSalesReport> weeklyOutletSalesReports,
            IReportService<MonthlyOutletSalesReport> monthlyOutletSalesReports,
            IReportService<PerDayReport> perDayReports
        #endregion
            )
        {
            _assetReport = assetReport;
            _liabilityReport = liabilityReport;
            _dailyIncomeReports = dailyIncomeReports;
            _weeklyIncomeReports = weeklyIncomeReports;
            _monthlyIncomeReports = monthlyIncomeReports;
            _dailyExpenseReports = dailyExpenseReports;
            _weeklyExpenseReports = weeklyExpenseReports;
            _monthlyExpenseReports = monthlyExpenseReports;
            _dailyPayableReports = dailyPayableReports;
            _weeklyPayableReports = weeklyPayableReports;
            _monthlyPayableReports = monthlyPayableReports;
            _dailyReceivableReports = dailyReceivableReports;
            _weeklyReceivableReports = weeklyReceivableReports;
            _monthlyReceivableReports = monthlyReceivableReports;
            _dailyOutletSalesReports = dailyOutletSalesReports;
            _weeklyOutletSalesReports = weeklyOutletSalesReports;
            _monthlyOutletSalesReports = monthlyOutletSalesReports;
            _perDayReports = perDayReports;
        }

        public async Task UpdateChartData()
        {
            Asset = (await _assetReport.Get()).FirstOrDefault();

            Liability = (await _liabilityReport.Get()).FirstOrDefault();

            Income = new IncomeReports
            {
                Daily = (await _dailyIncomeReports.Get()).FirstOrDefault(),
                Weekly = (await _weeklyIncomeReports.Get()).FirstOrDefault(),
                Monthly = (await _monthlyIncomeReports.Get()).FirstOrDefault()
            };

            Expense = new ExpenseReports
            {
                Daily = (await _dailyExpenseReports.Get()).FirstOrDefault(),
                Weekly = (await _weeklyExpenseReports.Get()).FirstOrDefault(),
                Monthly = (await _monthlyExpenseReports.Get()).FirstOrDefault()
            };

            Payable = new PayableReports
            {
                Daily = (await _dailyPayableReports.Get()).FirstOrDefault(),
                Weekly = (await _weeklyPayableReports.Get()).FirstOrDefault(),
                Monthly = (await _monthlyPayableReports.Get()).FirstOrDefault()
            };

            Receivable = new ReceivableReports
            {
                Daily = (await _dailyReceivableReports.Get()).FirstOrDefault(),
                Weekly = (await _weeklyReceivableReports.Get()).FirstOrDefault(),
                Monthly = (await _monthlyReceivableReports.Get()).FirstOrDefault()
            };

            OutletSales = new OutletSalesReport
            {
                Daily = await _dailyOutletSalesReports.Get(),
                Weekly = await _weeklyOutletSalesReports.Get(),
                Monthly = await _monthlyOutletSalesReports.Get()
            };

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
                    Income,
                    Expense,
                    Payable,
                    Receivable,
                    OutletSales,
                    PerDayReports
                });
        }

        public async Task RefreshData() 
            => await InitChartData();
    }
}
