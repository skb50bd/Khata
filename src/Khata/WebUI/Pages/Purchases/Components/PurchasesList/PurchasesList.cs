using System.Collections.Generic;

using DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Purchases.Components.PurchasesList
{
    public class PurchasesList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<PurchaseDto> purchases)
        {
            return View(nameof(PurchasesList), purchases);
        }
    }
}
