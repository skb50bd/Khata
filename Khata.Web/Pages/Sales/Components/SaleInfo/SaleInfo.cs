using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Sales.Components.SaleInfo
{
    public class SaleInfo : ViewComponent
    {
        public IViewComponentResult Invoke(SaleDto sale)
        {
            return View(nameof(SaleInfo), sale);
        }

    }
}
