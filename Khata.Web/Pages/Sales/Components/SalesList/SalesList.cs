using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Sales.Components.SalesList
{
    public class SalesList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<SaleDto> sales)
        {
            return View(nameof(SalesList), sales);
        }
    }
}
