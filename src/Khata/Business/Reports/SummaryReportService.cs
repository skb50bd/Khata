using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstractions;
using Business.PageFilterSort;
using Domain;
using Domain.Reports;
using DTOs;

namespace Business.Reports;

public class SummaryReportService
    : IReportService<Summary>
{
    #region Dependencies
    private readonly PfService _pf;
    private readonly IOutletService _outlets;
    private readonly ISaleService _sales;
    private readonly IDebtPaymentService _debtPayments;
    private readonly IPurchaseReturnService _purchaseReturns;

    private readonly ITransactionsService _transaction;

    private readonly IExpenseService _expenses;
    private readonly IPurchaseService _purchases;
    private readonly ISupplierPaymentService _supplierPayments;
    private readonly ISalaryPaymentService _salaryPaymets;
    private readonly ISalaryIssueService _salaryIssues;
    private readonly IRefundService _refunds;
    #endregion

    public SummaryReportService(
        #region Injected Dependencies
        PfService pf,
        IOutletService outlets,
        ISaleService sales,
        IDebtPaymentService debtPayments,
        IPurchaseReturnService purchaseReturns,
        ITransactionsService transaction,
        IExpenseService expenses,
        IPurchaseService purchases,
        ISupplierPaymentService supplierPayments,
        ISalaryPaymentService salaryPaymets,
        IRefundService refunds,
        ISalaryIssueService salaryIssues
        #endregion
    )
    {
        _pf = pf;
        _outlets = outlets;
        _sales = sales;
        _debtPayments = debtPayments;
        _purchaseReturns = purchaseReturns;
        _transaction = transaction;
        _expenses = expenses;
        _purchases = purchases;
        _supplierPayments = supplierPayments;
        _salaryPaymets = salaryPaymets;
        _refunds = refunds;
        _salaryIssues = salaryIssues;
    }

    public async Task<Summary> Get()
    {
        await Load();

        return new Summary
        {
            StartTime = FromDate,
            EndTime   = ToDate,
            CashIn = Sales.Sum(s => s.PaymentPaid)
                     + DebtReceivedAmount
                     + DepositsTotal
                     + PurchaseReturnAmount,
            CashOut = PurchasesPaid
                      + ExpenseTotal
                      + SupplierPaymentsAmount
                      + RefundsCashBack
                      + SalaryPaid,
            NewPayable = PurchasesDue
                         + SalaryIssueAmount
                         + CustomerAdvancePayment,
            NewReceivable = SalesDue
                            + SupplierAdvancePayment
                            + EmployeeAdvancePayment,
            SaleCount         = SalesCount,
            SaleReceives      = Sales.Sum(s => s.PaymentPaid),
            SalesWithDueCount = Sales.Count(s => s.PaymentDue > 0),
            SalesNewDue       = SalesDue,
            SalesProfit       = SalesProfit,
            ReceivedDebt      = DebtReceivedAmount,
            TotalExpense      = ExpenseTotal
        };
    }

    public async Task Load()
    {
        var pf = _pf.CreateNewPf("", 1, int.MaxValue);
        Outlets = await _outlets.Get();
        Sales = await _sales.Get(0, pf, FromDate, ToDate);
        DebtPayments = await _debtPayments.Get(pf, FromDate, ToDate);
        Deposits = (await _transaction.GetDeposits(FromDate, ToDate))
            .Where(d => d.TableName == nameof(Deposit))
            .ToList();
        PurchaseReturns       = await _purchaseReturns.Get(pf, FromDate, ToDate);
        Refunds               = await _refunds.Get(pf, FromDate, ToDate);
        Expenses              = await _expenses.Get(pf, FromDate, ToDate);
        Purchases             = await _purchases.Get(pf, FromDate, ToDate);
        SupplierPayments      = await _supplierPayments.Get(pf, FromDate, ToDate);
        SalaryPayments        = await _salaryPaymets.Get(pf, FromDate, ToDate);
        Withdrawals           = (await _transaction.GetWithdrawals(FromDate, ToDate))
            .Where(w => w.TableName == nameof(Withdrawal))
            .ToList();

        SalaryIssues = await _salaryIssues.Get(pf, FromDate, ToDate);


        foreach (var o in Outlets)
        {
            o.Sales = Sales.Where(s
                => s.OutletId == o.Id).ToList();
        }
    }

    #region Date Range

    public DateTime FromDate => DateTime.Today;
    public DateTime ToDate => DateTime.Now;
    #endregion

    #region Data Properties
    #region Outlets
    public IEnumerable<OutletDto> Outlets { get; set; }
    #endregion

    #region Sales
    public IEnumerable<SaleDto> Sales { get; set; }
        = new List<SaleDto>();

    [Display(Name = "Sales Count")]
    public int SalesCount
        => Sales?.Count() ?? 0;

    [Display(Name = "Cost of Goods Sold")]
    [DataType(DataType.Currency)]
    public decimal CostOfGoodsSold
        => Sales?.SelectMany(s => s.Cart)
            .Sum(s => s.NetPurchasePrice) ?? 0M;

    [Display(Name = "Sold Price of Goods")]
    [DataType(DataType.Currency)]
    public decimal PriceOfGoodsSold
        => Sales?.Sum(s => s.PaymentTotal) ?? 0M;

    [Display(Name = "Sales Profit")]
    [DataType(DataType.Currency)]
    public decimal SalesProfit
        => Sales?.Sum(s => s.Profit) ?? 0M;

    [Display(Name = "Sales Due")]
    [DataType(DataType.Currency)]
    public decimal SalesDue
        => Sales?.Sum(s => s.PaymentDue) ?? 0M;

    [Display(Name = "Profit Received (Profit - Due)",
        ShortName = "Received Profit")]
    [DataType(DataType.Currency)]
    public decimal SalesProfitReceived
        => SalesProfit - SalesDue;
    #endregion

    #region Debt Payments
    public IEnumerable<DebtPaymentDto> DebtPayments { get; set; }
        = new List<DebtPaymentDto>();

    [Display(Name = "Debt Payments Count")]
    public int DebtPaymentsCount
        => DebtPayments?.Count() ?? 0;

    [Display(Name = "Debt Received Amount")]
    [DataType(DataType.Currency)]
    public decimal DebtReceivedAmount
        => DebtPayments?.Sum(d => d.Amount) ?? 0M;

    public decimal CustomerAdvancePayment =>
        DebtPayments?.Sum(
            d => d.DebtAfter < 0M
                ? d.DebtBefore < 0
                    ? d.Amount
                    : d.DebtAfter
                : 0M)
        ?? 0M;
    #endregion

    #region Purchase Returns
    public IEnumerable<PurchaseReturnDto> PurchaseReturns { get; set; }
        = new List<PurchaseReturnDto>();

    [Display(Name = "Purchase Returns Count")]
    public int PurchaseReturnsCount
        => PurchaseReturns?.Count() ?? 0;

    [Display(Name = "Purchase Returns Cash Back")]
    [DataType(DataType.Currency)]
    public decimal PurchaseReturnAmount
        => PurchaseReturns?.Sum(pr => pr.CashBack) ?? 0M;
    #endregion

    #region Deposits
    public IEnumerable<Deposit> Deposits { get; set; }
        = new List<Deposit>();

    [Display(Name = "Deposits Count")]
    public int DepositsCount
        => Deposits?.Count() ?? 0;

    [Display(Name = "Deposits Total")]
    [DataType(DataType.Currency)]
    public decimal DepositsTotal
        => Deposits?.Sum(d => d.Amount) ?? 0M;
    #endregion

    #region Expenses
    public IEnumerable<ExpenseDto> Expenses { get; set; }
        = new List<ExpenseDto>();

    [Display(Name = "Expenses Count")]
    public int ExpensesCount => Expenses?.Count() ?? 0;

    [Display(Name = "Expense Total")]
    [DataType(DataType.Currency)]
    public decimal ExpenseTotal
        => Expenses?.Sum(e => e.Amount) ?? 0M;
    #endregion

    #region Purchases 
    public IEnumerable<PurchaseDto> Purchases { get; set; }
        = new List<PurchaseDto>();

    [Display(Name = "Purchases Count")]
    public int PurchasesCount
        => Purchases?.Count() ?? 0;

    [Display(Name = "Cost of Goods Purchased")]
    [DataType(DataType.Currency)]
    public decimal CostOfGoodsPurchased
        => Purchases?.Sum(p => p.PaymentTotal) ?? 0M;

    [Display(Name = "Purchases Due")]
    [DataType(DataType.Currency)]
    public decimal PurchasesDue
        => Purchases?.Sum(p => p.PaymentDue) ?? 0M;

    [Display(Name = "Purchase Paid")]
    [DataType(DataType.Currency)]
    public decimal PurchasesPaid
        => Purchases?.Sum(p => p.PaymentPaid) ?? 0M;
    #endregion

    #region Supplier Payments 
    public IEnumerable<SupplierPaymentDto> SupplierPayments { get; set; }
        = new List<SupplierPaymentDto>();

    [Display(Name = "Supplier Payments Count")]
    public int SupplierPaymentsCount
        => SupplierPayments?.Count() ?? 0;

    [Display(Name = "Supplier Payments Amount")]
    [DataType(DataType.Currency)]
    public decimal SupplierPaymentsAmount
        => SupplierPayments?.Sum(s => s.Amount) ?? 0M;

    public decimal SupplierAdvancePayment =>
        SupplierPayments?.Sum(
            d => d.PayableAfter < 0M
                ? d.PayableBefore < 0
                    ? d.Amount
                    : d.PayableAfter
                : 0M)
        ?? 0M;
    #endregion

    #region Salary Payments
    public IEnumerable<SalaryPaymentDto> SalaryPayments { get; set; }
        = new List<SalaryPaymentDto>();

    [Display(Name = "Salary Payments Count")]
    public int SalaryPaymentsCount
        => SalaryPayments?.Count() ?? 0;

    [Display(Name = "Salary Paid")]
    [DataType(DataType.Currency)]
    public decimal SalaryPaid
        => SalaryPayments?.Sum(sp => sp.Amount) ?? 0M;

    public decimal EmployeeAdvancePayment =>
        SalaryPayments?.Sum(
            d => d.BalanceAfter < 0M
                ? d.BalanceBefore < 0
                    ? d.Amount
                    : d.BalanceAfter
                : 0M)
        ?? 0M;
    #endregion

    #region Refunds
    public IEnumerable<RefundDto> Refunds { get; set; }
        = new List<RefundDto>();

    [Display(Name = "Refunds Count")]
    public int RefundsCount
        => Refunds?.Count() ?? 0;

    [Display(Name = "Refunds Cash Back")]
    [DataType(DataType.Currency)]
    public decimal RefundsCashBack
        => Refunds?.Sum(r => r.CashBack) ?? 0M;
    #endregion

    #region Withdrawals
    public IEnumerable<Withdrawal> Withdrawals { get; set; }
        = new List<Withdrawal>();

    [Display(Name = "Withdrawals Count")]
    public int WithdrawalsCount
        => Withdrawals?.Count() ?? 0;

    [Display(Name = "Withdrawals Total")]
    [DataType(DataType.Currency)]
    public decimal WithdrawalsTotal
        => Withdrawals?.Sum(w => w.Amount) ?? 0;
    #endregion

    #region SalaryIssues

    public IEnumerable<SalaryIssueDto> SalaryIssues { get; set; }

    public int SalaryIssuesCount =>
        SalaryIssues?.Count() ?? 0;

    public decimal SalaryIssueAmount =>
        SalaryIssues?.Sum(si => si.Amount) ?? 0M;

    #endregion

    #endregion
}