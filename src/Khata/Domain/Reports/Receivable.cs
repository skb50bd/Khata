using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Reports
{
    public class Receivable : Report
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

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
}
