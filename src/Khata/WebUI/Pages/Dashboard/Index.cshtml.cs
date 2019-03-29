using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Business.Reports;

using Microsoft.AspNetCore.Mvc.RazorPages;

using Queries;

namespace WebUI.Pages.Dashboard
{
    public class IndexModel : PageModel
    {
        #region Dependencies
        private readonly IReportService<AssetReport> _assetReports;
        private readonly IReportService<LiabilityReport> _liabilityReports;
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
        //private readonly IIndividualReportService<DailyOutletSalesReport> _dailyOutletSalesReports;
        //private readonly IIndividualReportService<WeeklyOutletSalesReport> _weeklyOutletSalesReports;
        //private readonly IIndividualReportService<MonthlyOutletSalesReport> _monthlyOutletSalesReports;
        private readonly IReportService<PerDayReport> _perDayReports;
        #endregion

        #region Data Properties (From QUERIES)
        public AssetReport AssetReport { get; set; }
        public LiabilityReport LiabilityReport { get; set; }
        public IncomeReports IncomeReports { get; set; }
        public ExpenseReports ExpenseReports { get; set; }
        public PayableReports Payable { get; set; }
        public ReceivableReports Receivable { get; set; }
        //public OutletSalesReport OutletSales { get; set; }
        public IEnumerable<PerDayReport> PerDayReports { get; set; } 
        #endregion

        public IndexModel(
        #region Injected Dependencies
        IReportService<AssetReport> assetReports,
            IReportService<LiabilityReport> liabilityReports,
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
            //IIndividualReportService<DailyOutletSalesReport> dailyOutletSalesReports,
            //IIndividualReportService<WeeklyOutletSalesReport> weeklyOutletSalesReports,
            //IIndividualReportService<MonthlyOutletSalesReport> monthlyOutletSalesReports,
            IReportService<PerDayReport> perDayReports 
        #endregion
            )
        {
            _assetReports = assetReports;
            _liabilityReports = liabilityReports;
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
            _perDayReports = perDayReports;
        }

        public async Task OnGetAsync()
        {
            var sw = new Stopwatch();
            sw.Start();

            AssetReport = (await _assetReports.Get()).FirstOrDefault();
            LiabilityReport = (await _liabilityReports.Get()).FirstOrDefault();
            IncomeReports = new IncomeReports
            {
                Daily = (await _dailyIncomeReports.Get()).FirstOrDefault(),
                Weekly = (await _weeklyIncomeReports.Get()).FirstOrDefault(),
                Monthly = (await _monthlyIncomeReports.Get()).FirstOrDefault()
            };

            ExpenseReports = new ExpenseReports
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

            PerDayReports = await _perDayReports.Get();

            sw.Stop();
            Debug.WriteLine($"Took Time {sw.ElapsedMilliseconds}");
        }
    }
    public struct IncomeReportWithSpan
    {
        public IncomeBase Income { get; set; }
        public string Span { get; set; }
    }
    public struct ExpenseReportWithSpan
    {
        public ExpenseBase Expense { get; set; }
        public string Span { get; set; }
    }

    public struct PayableWithSpan
    {
        public PayableBase Payable { get; set; }
        public string Span { get; set; }
    }

    public struct ReceivableWithSpan
    {
        public ReceivableBase Receivable { get; set; }
        public string Span { get; set; }
    }

    public struct OutletSalesWithSpan
    {
        public IEnumerable<OutletSalesBase> OutletSales { get; set; }
        public string Span { get; set; }
    }
}