using System.ComponentModel.DataAnnotations;

namespace Queries
{
    public abstract class ReceivableBase : Report
    {
        public int      SalesDueCount               { get; set; }

        [DataType(DataType.Currency)]
        public decimal  SalesDueAmount              { get; set; }

        public int      SupplierOverPaymentCount    { get; set; }

        [DataType(DataType.Currency)]
        public decimal  SupplierOverPaymentAmount   { get; set; }

        public int      SalaryOverPaymentCount      { get; set; }

        [DataType(DataType.Currency)]
        public decimal  SalaryOverPaymentAmount     { get; set; }
    }
    public class DailyReceivableReport : ReceivableBase { }
    public class WeeklyReceivableReport : ReceivableBase { }
    public class MonthlyReceivableReport : ReceivableBase { }

    public class ReceivableReports
    {
        public DailyReceivableReport Daily { get; set; }
        public WeeklyReceivableReport Weekly { get; set; }
        public MonthlyReceivableReport Monthly { get; set; }
    }
}
