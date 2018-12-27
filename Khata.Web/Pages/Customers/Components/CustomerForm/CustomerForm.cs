using Khata.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Shared.Components.CustomerForm
{
    public class CustomerForm : ViewComponent
    {
        public IViewComponentResult Invoke(CustomerViewModel customer)
        {
            return View("Default", customer);
        }
    }
}
