using Khata.Queries;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Khata.Data.Persistence
{
    public partial class KhataContext : IdentityDbContext
    {
        public virtual DbQuery<CustomerReport> CustomerReports { get; set; }
        public virtual DbQuery<SupplierReport> SupplierReports { get; set; }
        public virtual DbQuery<AssetReport> AssetReport { get; set; }
        public virtual DbQuery<LiabilityReport> LiabilityReport { get; set; }
        public virtual DbQuery<DailyIncomeReport> DailyIncomeReport { get; set; }
        public virtual DbQuery<DailyExpenseReport> DailyExpenseReport { get; set; }
        public virtual DbQuery<DailyPayableReport> DailyPayableReport { get; set; }
        public virtual DbQuery<DailyReceivableReport> DailyReceivableReport { get; set; }
        public virtual DbQuery<DailyOutletSalesReport> DailyOutletSalesReport { get; set; }
        public virtual DbQuery<WeeklyIncomeReport> WeeklyIncomeReport { get; set; }
        public virtual DbQuery<WeeklyExpenseReport> WeeklyExpenseReport { get; set; }
        public virtual DbQuery<WeeklyPayableReport> WeeklyPayableReport { get; set; }
        public virtual DbQuery<WeeklyReceivableReport> WeeklyReceivableReport { get; set; }
        public virtual DbQuery<WeeklyOutletSalesReport> WeeklyOutletSalesReport { get; set; }
        public virtual DbQuery<MonthlyIncomeReport> MonthlyIncomeReport { get; set; }
        public virtual DbQuery<MonthlyExpenseReport> MonthlyExpenseReport { get; set; }
        public virtual DbQuery<MonthlyPayableReport> MonthlyPayableReport { get; set; }
        public virtual DbQuery<MonthlyReceivableReport> MonthlyReceivableReport { get; set; }
        public virtual DbQuery<MonthlyOutletSalesReport> MonthlyOutletSalesReport { get; set; }
        public virtual DbQuery<PerDayReport> PerDayReports { get; set; }
    }
}
