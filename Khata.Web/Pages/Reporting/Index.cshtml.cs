using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

using Brotal.Extensions;

using Khata.Domain;
using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Reporting
{
    public class IndexModel : PageModel
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
        private readonly IRefundService _refunds; 
        #endregion

        public IndexModel(
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
            IRefundService refunds 
        #endregion
        )
        {
            _pf               = pf;
            _outlets          = outlets;
            _sales            = sales;
            _debtPayments     = debtPayments;
            _purchaseReturns  = purchaseReturns;
            _transaction      = transaction;
            _expenses         = expenses;
            _purchases        = purchases;
            _supplierPayments = supplierPayments;
            _salaryPaymets    = salaryPaymets;
            _refunds          = refunds;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            FromText = DateTime.Today.LocalDate();
            ToText = FromText;
            await Load();
            return Page();
        }

        public async Task<IActionResult> OnPostWithDateRangeAsync()
        {
            if (string.IsNullOrWhiteSpace(FromText)
                || string.IsNullOrWhiteSpace(ToText))
            {
                return Page();
            }

            await Load();

            return Page();
        }

        public async Task Load()
        {
            var pf           = _pf.CreateNewPf("");
            Outlets          = await _outlets.Get();
            Sales            = await _sales.Get(0, pf, FromDate, ToDate);
            DebtPayments     = await _debtPayments.Get(pf, FromDate, ToDate);
            Deposits         = (await _transaction.GetDeposits(FromDate, ToDate))
                                    .Where(d => d.TableName == nameof(Deposit));
            PurchaseReturns  = await _purchaseReturns.Get(pf, FromDate, ToDate);
            Refunds          = await _refunds.Get(pf, FromDate, ToDate);
            Expenses         = await _expenses.Get(pf, FromDate, ToDate);
            Purchases        = await _purchases.Get(pf, FromDate, ToDate);
            SupplierPayments = await _supplierPayments.Get(pf, FromDate, ToDate);
            SalaryPayments   = await _salaryPaymets.Get(pf, FromDate, ToDate);
            Withdrawals      = (await _transaction.GetWithdrawals(FromDate, ToDate))
                                    .Where(w => w.TableName == nameof(Withdrawal));

            foreach (var o in Outlets)
            {
                o.Sales = Sales.Where(s => s.OutletId == o.Id).ToList();
            }
        }

        #region Date Time Values
        [BindProperty]
        [Display(Name = "Start Date")]
        public string FromText { get; set; }
        [BindProperty]
        [Display(Name = "End Date")]
        public string ToText { get; set; }

        public DateTime FromDate =>
            (DateTime)FromText.TryParseDate(DateTime.MinValue);
        public DateTime ToDate =>
            ((DateTime)ToText.TryParseDate(DateTime.MaxValue))
                .AddSeconds(86_399); // Till 23:59:59 
        #endregion

        #region Data Properties (from Database)

        #region Outlets
        public IEnumerable<OutletDto> Outlets { get; set; }
        #endregion

        #region Sales
        public IEnumerable<SaleDto> Sales { get; set; } = new List<SaleDto>();

        [Display(Name = "Sales Count")]
        public int SalesCount => Sales?.Count() ?? 0;

        [Display(Name = "Cost of Goods Sold")]
        [DataType(DataType.Currency)]
        public decimal CostOfGoodsSold => Sales?.SelectMany(s => s.Cart).Sum(s => s.NetPurchasePrice) ?? 0M;

        [Display(Name = "Sold Price of Goods")]
        [DataType(DataType.Currency)]
        public decimal PriceOfGoodsSold => Sales?.Sum(s => s.PaymentTotal) ?? 0M;

        [Display(Name = "Sales Profit")]
        [DataType(DataType.Currency)]
        public decimal SalesProfit => Sales?.Sum(s => s.Profit) ?? 0M;

        [Display(Name = "Sales Due")]
        [DataType(DataType.Currency)]
        public decimal SalesDue => Sales?.Sum(s => s.PaymentDue) ?? 0M;

        [Display(Name = "Profit Received (Profit - Due)",
            ShortName = "Received Profit")]
        [DataType(DataType.Currency)]
        public decimal SalesProfitReceived => SalesProfit - SalesDue;
        #endregion

        #region Debt Payments
        public IEnumerable<DebtPaymentDto> DebtPayments { get; set; } = new List<DebtPaymentDto>();

        [Display(Name = "Debt Payments Count")]
        public int DebtPaymentsCount => DebtPayments?.Count() ?? 0;

        [Display(Name = "Debt Received Amount")]
        [DataType(DataType.Currency)]
        public decimal DebtReceivedAmount => DebtPayments?.Sum(d => d.Amount) ?? 0M;
        #endregion

        #region Purchase Returns
        public IEnumerable<PurchaseReturnDto> PurchaseReturns { get; set; } = new List<PurchaseReturnDto>();

        [Display(Name = "Purchase Returns Count")]
        public int PurchaseReturnsCount => PurchaseReturns?.Count() ?? 0;

        [Display(Name = "Purchase Returns Cash Back")]
        [DataType(DataType.Currency)]
        public decimal PurchaseReturnAmount => PurchaseReturns?.Sum(pr => pr.CashBack) ?? 0M;
        #endregion

        #region Deposits
        public IEnumerable<Deposit> Deposits { get; set; } = new List<Deposit>();

        [Display(Name = "Deposits Count")]
        public int DepositsCount => Deposits?.Count() ?? 0;

        [Display(Name = "Deposits Total")]
        [DataType(DataType.Currency)]
        public decimal DepositsTotal => Deposits?.Sum(d => d.Amount) ?? 0M;
        #endregion

        #region Expenses
        public IEnumerable<ExpenseDto> Expenses { get; set; } = new List<ExpenseDto>();

        [Display(Name = "Expenses Count")]
        public int ExpensesCount => Expenses?.Count() ?? 0;

        [Display(Name = "Expense Total")]
        [DataType(DataType.Currency)]
        public decimal ExpenseTotal => Expenses?.Sum(e => e.Amount) ?? 0M;
        #endregion

        #region Purchases 
        public IEnumerable<PurchaseDto> Purchases { get; set; } = new List<PurchaseDto>();

        [Display(Name = "Purchases Count")]
        public int PurchasesCount => Purchases?.Count() ?? 0;

        [Display(Name = "Cost of Goods Purchased")]
        [DataType(DataType.Currency)]
        public decimal CostOfGoodsPurchased => Purchases?.Sum(p => p.PaymentTotal) ?? 0M;

        [Display(Name = "Purchases Due")]
        [DataType(DataType.Currency)]
        public decimal PurchasesDue => Purchases?.Sum(p => p.PaymentDue) ?? 0M;

        [Display(Name = "Purchase Paid")]
        [DataType(DataType.Currency)]
        public decimal PurchasesPaid => Purchases?.Sum(p => p.PaymentPaid) ?? 0M;
        #endregion

        #region Supplier Payments 
        public IEnumerable<SupplierPaymentDto> SupplierPayments { get; set; }
            = new List<SupplierPaymentDto>();

        [Display(Name = "Supplier Payments Count")]
        public int SupplierPaymentsCount => SupplierPayments?.Count() ?? 0;

        [Display(Name = "Supplier Payments Amount")]
        [DataType(DataType.Currency)]
        public decimal SupplierPaymentsAmount => SupplierPayments?.Sum(s => s.Amount) ?? 0M;
        #endregion

        #region Salary Payments
        public IEnumerable<SalaryPaymentDto> SalaryPayments { get; set; } = new List<SalaryPaymentDto>();

        [Display(Name = "Salary Payments Count")]
        public int SalaryPaymentsCount => SalaryPayments?.Count() ?? 0;

        [Display(Name = "Salary Paid")]
        [DataType(DataType.Currency)]
        public decimal SalaryPaid => SalaryPayments?.Sum(sp => sp.Amount) ?? 0M;
        #endregion

        #region Refunds
        public IEnumerable<RefundDto> Refunds { get; set; } = new List<RefundDto>();

        [Display(Name = "Refunds Count")]
        public int RefundsCount => Refunds?.Count() ?? 0;

        [Display(Name = "Refunds Cash Back")]
        [DataType(DataType.Currency)]
        public decimal RefundsCashBack => Refunds?.Sum(r => r.CashBack) ?? 0M;
        #endregion

        #region Withdrawals
        public IEnumerable<Withdrawal> Withdrawals { get; set; } = new List<Withdrawal>();

        [Display(Name = "Withdrawals Count")]
        public int WithdrawalsCount => Withdrawals?.Count() ?? 0;

        [Display(Name = "Withdrawals Total")]
        [DataType(DataType.Currency)]
        public decimal WithdrawalsTotal => Withdrawals?.Sum(w => w.Amount) ?? 0;
        #endregion 
        
        #endregion
    }
}