using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Outlets.Components.OutletForm
{
    public class OutletForm : ViewComponent
    {
        public IViewComponentResult Invoke(OutletViewModel outlet)
        {
            return View(nameof(OutletForm), outlet);
        }
    }
}
