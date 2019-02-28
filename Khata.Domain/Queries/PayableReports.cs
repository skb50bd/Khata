using System.ComponentModel.DataAnnotations;

namespace Khata.Queries
{
    public abstract class PayableBase : Report
    {
        public int PurchaseDueCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal PurchaseDueAmount { get; set; }

        public int SalaryIssueCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal SalaryIssueAmount { get; set; }

        public int DebtOverPaymentCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal DebtOverPaymentAmount { get; set; }
    }
    public class DailyPayableReport : PayableBase { }
    public class WeeklyPayableReport : PayableBase { }
    public class MonthlyPayableReport : PayableBase { }

    public class PayableReports
    {
        public DailyPayableReport Daily { get; set; }
        public WeeklyPayableReport Weekly { get; set; }
        public MonthlyPayableReport Monthly { get; set; }
    }
}
