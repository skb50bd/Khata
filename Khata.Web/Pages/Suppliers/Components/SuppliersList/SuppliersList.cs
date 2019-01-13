using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Suppliers.Components.SuppliersList
{
    public class SuppliersList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<SupplierDto> suppliers)
        {
            return View(nameof(SuppliersList), suppliers);
        }
    }
}
