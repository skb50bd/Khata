using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Sales.Components.SaleForm
{
    public class SaleForm : ViewComponent
    {
        public IViewComponentResult Invoke(SaleViewModel sale)
        {
            return View(nameof(SaleForm), sale);
        }
    }
}
