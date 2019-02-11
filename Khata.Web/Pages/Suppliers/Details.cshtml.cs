using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Queries;
using Khata.Services.CRUD;
using Khata.Services.Reports;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace WebUI.Pages.Suppliers
{
    public class DetailsModel : PageModel
    {
        private readonly ISupplierService _suppliers;
        private readonly IPurchaseService _purchases;
        private readonly ISupplierPaymentService _supplierPayments;
        private readonly IPurchaseReturnService _purchaseReturns;
        private readonly IIndividualReportService<SupplierReport> _reports;

        public DetailsModel(
            ISupplierService suppliers,
            IPurchaseService purchases,
            ISupplierPaymentService supplierPayments,
            IPurchaseReturnService purchaseReturns,
            IIndividualReportService<SupplierReport> reports)
        {
            _suppliers = suppliers;
            _purchases = purchases;
            _supplierPayments = supplierPayments;
            _purchaseReturns = purchaseReturns;
            _reports = reports;
        }

        public SupplierDto Supplier { get; set; }
        public SupplierReport Report { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Supplier = await _suppliers.Get((int)id);
            Report = await _reports.Get((int)id);

            if (Supplier is null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnGetPurchasesAsync(int id)
        {
            if (!await _suppliers.Exists(id))
            {
                return NotFound();
            }
            var purchases = await _purchases.GetSupplierPurchases(id);

            return new PartialViewResult
            {
                ViewName = "_PurchasesList",
                ViewData = new ViewDataDictionary<IEnumerable<PurchaseDto>>(ViewData, purchases)
            };
        }

        public async Task<IActionResult> OnGetSupplierPaymentsAsync(int id)
        {
            if (!await _suppliers.Exists(id))
            {
                return NotFound();
            }
            var supplierPayments = await _supplierPayments.GetSupplierPayments(id);

            return new PartialViewResult
            {
                ViewName = "_SupplierPaymentsList",
                ViewData = new ViewDataDictionary<IEnumerable<SupplierPaymentDto>>(ViewData, supplierPayments)
            };
        }

        public async Task<IActionResult> OnGetPurchaseReturnsAsync(int id)
        {
            if (!await _suppliers.Exists(id))
            {
                return NotFound();
            }
            var purchaseReturns = await _purchaseReturns.GetSupplierPurchaseReturns(id);

            return new PartialViewResult
            {
                ViewName = "_PurchaseReturnsList",
                ViewData = new ViewDataDictionary<IEnumerable<PurchaseReturnDto>>(ViewData, purchaseReturns)
            };
        }

        public async Task<IActionResult> OnGetBriefAsync(int supplierId)
        {
            if (!await _suppliers.Exists(supplierId))
            {
                return NotFound();
            }
            var supplier = await _suppliers.Get(supplierId);
            return new PartialViewResult
            {
                ViewName = "_SupplierBriefInfo",
                ViewData = new ViewDataDictionary<SupplierDto>(ViewData, supplier)
            };
        }
    }
}
