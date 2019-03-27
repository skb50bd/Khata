using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Queries;

namespace Data.Persistence
{
    public partial class KhataContext 
    {
        public DbQuery<CustomerReport> CustomerReports { get; set; }
        public DbQuery<SupplierReport> SupplierReports { get; set; }
        public DbQuery<AssetReport> AssetReport { get; set; }
        public DbQuery<LiabilityReport> LiabilityReport { get; set; }
        public DbQuery<DailyIncomeReport> DailyIncomeReport { get; set; }
        public DbQuery<DailyExpenseReport> DailyExpenseReport { get; set; }
        public DbQuery<DailyPayableReport> DailyPayableReport { get; set; }
        public DbQuery<DailyReceivableReport> DailyReceivableReport { get; set; }
        public DbQuery<DailyOutletSalesReport> DailyOutletSalesReport { get; set; }
        public DbQuery<WeeklyIncomeReport> WeeklyIncomeReport { get; set; }
        public DbQuery<WeeklyExpenseReport> WeeklyExpenseReport { get; set; }
        public DbQuery<WeeklyPayableReport> WeeklyPayableReport { get; set; }
        public DbQuery<WeeklyReceivableReport> WeeklyReceivableReport { get; set; }
        public DbQuery<WeeklyOutletSalesReport> WeeklyOutletSalesReport { get; set; }
        public DbQuery<MonthlyIncomeReport> MonthlyIncomeReport { get; set; }
        public DbQuery<MonthlyExpenseReport> MonthlyExpenseReport { get; set; }
        public DbQuery<MonthlyPayableReport> MonthlyPayableReport { get; set; }
        public DbQuery<MonthlyReceivableReport> MonthlyReceivableReport { get; set; }
        public DbQuery<MonthlyOutletSalesReport> MonthlyOutletSalesReport { get; set; }
        public DbQuery<PerDayReport> PerDayReports { get; set; }
    }
}
