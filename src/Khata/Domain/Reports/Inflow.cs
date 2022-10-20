using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Reports;

public class Inflow : Report
{
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    [Display(Name = "Number of Sales")]
    public int SaleCount { get; set; }

    [DataType(DataType.Currency)]
    public decimal SaleReceived { get; set; }


    [DataType(DataType.Currency)]
    public decimal SaleProfit { get; set; }

    public int DebtPaymentCount { get; set; }

    [DataType(DataType.Currency)]
    public decimal DebtReceived { get; set; }

    public int PurchaseReturnsCount { get; set; }

    [DataType(DataType.Currency)]
    public decimal PurchaseReturnsReceived { get; set; }


    public int DepositsCount { get; set; }

    [DataType(DataType.Currency)]
    public decimal DepositAmount { get; set; }
}