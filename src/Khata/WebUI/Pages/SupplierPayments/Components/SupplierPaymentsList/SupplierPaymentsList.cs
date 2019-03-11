using System.Collections.Generic;

using DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.SupplierPayments.Components.SupplierPaymentsList
{
    public class SupplierPaymentsList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<SupplierPaymentDto> supplierPayments)
        {
            return View(nameof(SupplierPaymentsList), supplierPayments);
        }
    }
}
