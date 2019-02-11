using System.ComponentModel.DataAnnotations;

namespace Khata.Queries
{
    public class ExpenseBase : Report
    {
        public int PurchaseCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal PurchasePaid { get; set; }


        public int SupplierPaymentCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal SupplierPaymentAmount { get; set; }


        public int EmployeePaymentCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal EmployeePaymentAmount { get; set; }


        public int ExpenseCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal ExpenseAmount { get; set; }


        public int WithdrawalCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal WithdrawalAmount { get; set; }
    }

    public class DailyExpenseReport : ExpenseBase { }
    public class WeeklyExpenseReport : ExpenseBase { }
    public class MonthlyExpenseReport : ExpenseBase { }

    public class ExpenseReports
    {
        public DailyExpenseReport Daily { get; set; }
        public WeeklyExpenseReport Weekly { get; set; }
        public MonthlyExpenseReport Monthly { get; set; }
    }
}
