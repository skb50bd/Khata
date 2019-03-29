using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstractions;
using DTOs;
using Business.Reports;
using Domain.Reports;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Pages.Customers
{
    public class DetailsModel : PageModel
    {
        private readonly ICustomerService _customers;
        private readonly ISaleService _sales;
        private readonly IDebtPaymentService _debtPayments;
        private readonly IRefundService _refunds;
        private readonly IIndividualReportService<CustomerReport> _reports;
        public DetailsModel(ICustomerService customers,
            ISaleService sales,
            IDebtPaymentService debtPayments,
            IRefundService refunds,
            IIndividualReportService<CustomerReport> reports)
        {
            _customers = customers;
            _sales = sales;
            _debtPayments = debtPayments;
            _refunds = refunds;
            _reports = reports;
        }

        public CustomerDto Customer { get; set; }
        public CustomerReport Report { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Customer = await _customers.Get((int)id);
            Report = await _reports.Get((int)id);

            if (Customer is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetSalesAsync(int id)
        {
            if (!await _customers.Exists(id))
            {
                return NotFound();
            }
            var sales = await _sales.GetCustomerSales(id);

            return new PartialViewResult
            {
                ViewName = "_SalesList",
                ViewData = new ViewDataDictionary<IEnumerable<SaleDto>>(ViewData, sales)
            };
        }

        public async Task<IActionResult> OnGetDebtPaymentsAsync(int id)
        {
            if (!await _customers.Exists(id))
            {
                return NotFound();
            }
            var debtPayments = await _debtPayments.GetCustomerDebtPayments(id);

            return new PartialViewResult
            {
                ViewName = "_DebtPaymentsList",
                ViewData = new ViewDataDictionary<IEnumerable<DebtPaymentDto>>(ViewData, debtPayments)
            };
        }

        public async Task<IActionResult> OnGetRefundsAsync(int id)
        {
            if (!await _customers.Exists(id))
            {
                return NotFound();
            }
            var refunds = await _refunds.GetCustomerRefunds(id);

            return new PartialViewResult
            {
                ViewName = "_RefundsList",
                ViewData = new ViewDataDictionary<IEnumerable<RefundDto>>(ViewData, refunds)
            };
        }

        public async Task<IActionResult> OnGetBriefAsync(int customerId)
        {
            if (!await _customers.Exists(customerId))
            {
                return NotFound();
            }
            var customer = await _customers.Get(customerId);
            return new PartialViewResult
            {
                ViewName = "_CustomerBriefInfo",
                ViewData = new ViewDataDictionary<CustomerDto>(ViewData, customer)
            };
        }
    }
}
