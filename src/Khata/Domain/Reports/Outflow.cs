using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Reports;

public class Outflow : Report
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
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


    public int RefundCount { get; set; }

    [DataType(DataType.Currency)]
    public decimal RefundAmount { get; set; }

    public int WithdrawalCount { get; set; }

    [DataType(DataType.Currency)]
    public decimal WithdrawalAmount { get; set; }
}