using System;
using System.ComponentModel.DataAnnotations;

namespace Domain.Reports;

public class Summary: Report
{
    public DateTimeOffset StartTime { get; set; }

    public DateTimeOffset EndTime { get; set; }

    public SummaryType Type =>
        StartTime.Date == EndTime.Date
            ? SummaryType.Daily
            : EndTime - StartTime >= TimeSpan.FromDays(7)
                ? SummaryType.Weekly
                : SummaryType.Monthly;

    public DateTimeOffset GeneratedOn { get; set; } = Clock.Now;

    [DataType(DataType.Currency)]
    [Display(Name = "Cash In")]
    public decimal CashIn { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Cash Out")]
    public decimal CashOut { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "New Receivable")]
    public decimal NewReceivable { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "New Payable")]
    public decimal NewPayable { get; set; }

    [Display(Name = "Number of Sales")]
    public int SaleCount { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Sale Receives")]
    public decimal SaleReceives { get; set; }

    [Display(Name = "Number of Sales with Due")]
    public int SalesWithDueCount { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "New Receivable from Sales")]
    public decimal SalesNewDue { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Profit from Sales")]
    public decimal SalesProfit { get; set; }


    [DataType(DataType.Currency)]
    [Display(Name = "Debt Received")]
    public decimal ReceivedDebt { get; set; }

    [DataType(DataType.Currency)]
    [Display(Name = "Total Expense")]
    public decimal TotalExpense { get; set; }
}

public enum SummaryType
{
    Daily,
    Weekly,
    Monthly
}