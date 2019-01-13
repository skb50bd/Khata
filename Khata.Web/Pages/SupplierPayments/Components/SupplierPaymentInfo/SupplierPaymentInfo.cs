using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.SupplierPayments.Components.SupplierPaymentInfo
{
    public class SupplierPaymentInfo : ViewComponent
    {
        public IViewComponentResult Invoke(SupplierPaymentDto supplierPayment
            )
        {
            return View(nameof(SupplierPaymentInfo), supplierPayment);
        }

    }
}
