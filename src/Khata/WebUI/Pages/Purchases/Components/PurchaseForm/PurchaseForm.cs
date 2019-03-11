using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Purchases.Components.PurchaseForm
{
    public class PurchaseForm : ViewComponent
    {
        public IViewComponentResult Invoke(PurchaseViewModel purchase)
        {
            return View(nameof(PurchaseForm), purchase);
        }
    }
}
