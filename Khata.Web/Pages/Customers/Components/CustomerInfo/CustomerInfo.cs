using Khata.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Shared.Components.CustomerInfo
{
    public class CustomerInfo : ViewComponent
    {
        public IViewComponentResult Invoke(CustomerDto customer)
        {
            return View("Default", customer);
        }

    }
}
