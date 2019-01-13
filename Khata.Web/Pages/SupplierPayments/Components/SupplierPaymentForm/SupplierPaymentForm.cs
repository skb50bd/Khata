using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.SupplierPayments.Components.SupplierPaymentForm
{
    public class SupplierPaymentForm : ViewComponent
    {
        public IViewComponentResult Invoke(SupplierPaymentViewModel supplierPayment)
        {
            return View(nameof(SupplierPaymentForm), supplierPayment);
        }
    }
}
