using DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Outlets.Components.OutletInfo
{
    public class OutletInfo : ViewComponent
    {
        public IViewComponentResult Invoke(OutletDto outlet)
        {
            return View(nameof(OutletInfo), outlet);
        }

    }
}
