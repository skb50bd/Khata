using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Purchases.Components.PurchaseInfo
{
    public class PurchaseInfo : ViewComponent
    {
        public IViewComponentResult Invoke(PurchaseDto purchase)
        {
            return View(nameof(PurchaseInfo), purchase);
        }

    }
}
