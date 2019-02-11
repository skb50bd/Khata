using System.ComponentModel.DataAnnotations;

namespace Khata.Queries
{
    public class CustomerReport : IndividaulReport
    {
        public string FullName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public string Address { get; set; }

        [DataType(DataType.Currency)]
        public decimal Debt { get; set; }

        public int SalesCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal SalesWorth { get; set; }

        [DataType(DataType.Currency)]
        public decimal SaleReceives { get; set; }

        [DataType(DataType.Currency)]
        public decimal Profit { get; set; }

        public int DebtPaymentsCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal DebtPaymentReceives { get; set; }

        [DataType(DataType.Currency)]
        public decimal Delta => Profit - Debt;

        public int RefundCount { get; set; }

        [DataType(DataType.Currency)]
        public decimal RefundLoss { get; set; }

        [DataType(DataType.Currency)]
        public decimal RefundAmount { get; set; }
    }
}
