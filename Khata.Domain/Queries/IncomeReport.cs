using System.ComponentModel.DataAnnotations;

namespace Khata.Queries
{
    public class Income : Report
    {
        [Display(Name = "Number of Sales")]
        public int SaleCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal SaleReceived { get; set; }


        public int DebtPaymentCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal DebtReceived { get; set; }

        public int DepositsCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal DepositAmount { get; set; }

    }

    public class DailyIncomeReport : Income { }
    public class WeeklyIncomeReport : Income { }
    public class MonthlyIncomeReport : Income { }

    public class IncomeReports
    {
        public DailyIncomeReport Daily { get; set; }
        public WeeklyIncomeReport Weekly { get; set; }
        public MonthlyIncomeReport Monthly { get; set; }
    }
}
