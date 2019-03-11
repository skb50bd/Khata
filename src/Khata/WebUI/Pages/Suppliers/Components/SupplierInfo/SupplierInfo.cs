using DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Suppliers.Components.SupplierInfo
{
    public class SupplierInfo : ViewComponent
    {
        public IViewComponentResult Invoke(SupplierDto supplier)
        {
            return View(nameof(SupplierInfo), supplier);
        }

    }
}
