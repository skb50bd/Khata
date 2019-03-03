using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Khata.Queries
{
    public abstract class OutletSalesBase : IndividaulReport
    {
        [Display(Name = "Outlet")]
        public string OutletTitle { get; set; }

        [Display(Name = "Number of Sales")]
        public int SaleCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal SaleReceived { get; set; }

        [DataType(DataType.Currency)]
        public decimal SaleDue { get; set; }

        [DataType(DataType.Currency)]
        public decimal SaleProfit { get; set; }
    }

    public class DailyOutletSalesReport : OutletSalesBase { }
    public class WeeklyOutletSalesReport : OutletSalesBase { }
    public class MonthlyOutletSalesReport : OutletSalesBase { }

    public class OutletSalesReport
    {
        public IEnumerable<DailyOutletSalesReport> Daily { get; set; }
        public IEnumerable<WeeklyOutletSalesReport> Weekly { get; set; }
        public IEnumerable<MonthlyOutletSalesReport> Monthly { get; set; }
    }

}
