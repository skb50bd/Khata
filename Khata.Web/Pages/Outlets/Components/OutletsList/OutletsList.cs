using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Outlets.Components.OutletsList
{
    public class OutletsList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<OutletDto> outlets)
        {
            return View(nameof(OutletsList), outlets);
        }
    }
}
