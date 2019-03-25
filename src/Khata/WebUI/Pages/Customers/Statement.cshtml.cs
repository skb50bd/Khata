using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Brotal.Extensions;

using Business.CRUD;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Customers
{
    public class StatementModel : PageModel
    {
        private readonly ICustomerService _customers;
        private readonly ISaleService _sales;
        private readonly IDebtPaymentService _debtPayments;
        private readonly IRefundService _refunds;
        public StatementModel(ICustomerService customers,
            ISaleService sales,
            IDebtPaymentService debtPayments,
            IRefundService refunds)
        {
            _customers = customers;
            _sales = sales;
            _debtPayments = debtPayments;
            _refunds = refunds;
        }

        public DateTime ForDate => DateTime.Today;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public CustomerDto Customer { get; set; }
        public IEnumerable<SaleDto> Sales { get; set; }
        public IEnumerable<DebtPaymentDto> DebtPayments { get; set; }
        public IEnumerable<RefundDto> Refunds { get; set; }
        public IEnumerable<StatementElement> Elements =>
            Sales.Select(
                s => new StatementElement
                {
                    DateTime = s.Metadata.CreationTime,
                    Type = "Sale",
                    Total = s.PaymentTotal,
                    Paid = s.PaymentPaid,
                    Due = s.PaymentDue
                }
            ).Union(
                DebtPayments.Select(
                    d => new StatementElement
                    {
                        DateTime = d.Metadata.CreationTime,
                        Type = "Debt Payment",
                        Total = d.DebtBefore,
                        Paid = d.Amount,
                        Due = d.DebtAfter
                    }
                )
            ).Union(
                Refunds.Select(
                    r => new StatementElement
                    {
                        DateTime = r.Metadata.CreationTime,
                        Type = "Refund",
                        Total = r.TotalPrice,
                        Paid = r.CashBack + r.DebtRollback,
                        Due = 0M
                    }
                )
            ).OrderByDescending(e => e.DateTime);


        public async Task<IActionResult> OnGetAsync(int customerId, string fromDate, string toDate)
        {
            if (!await _customers.Exists(customerId))
                return BadRequest("Customer Not Found");

            FromDate = fromDate.ParseDate();
            ToDate = toDate.ParseDate();

            Customer = await _customers.Get(customerId);
            Sales = await _sales.GetCustomerSales(customerId, FromDate, ToDate);
            DebtPayments = await _debtPayments.GetCustomerDebtPayments(customerId, FromDate, ToDate);
            Refunds = await _refunds.GetCustomerRefunds(customerId, FromDate, ToDate);

            return Page();
        }

        public class StatementElement
        {
            public string Type { get; set; }
            public DateTimeOffset DateTime { get; set; }
            public decimal Total { get; set; }
            public decimal Paid { get; set; }
            public decimal Due { get; set; }
        }
    }
}