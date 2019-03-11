using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Suppliers.Components.SupplierForm
{
    public class SupplierForm : ViewComponent
    {
        public IViewComponentResult Invoke(SupplierViewModel supplier)
        {
            return View(nameof(SupplierForm), supplier);
        }
    }
}
