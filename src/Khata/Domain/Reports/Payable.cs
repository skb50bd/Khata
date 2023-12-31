﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Reports
{
    public class Payable : Report
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

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
}
