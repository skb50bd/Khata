using DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Customers.Components.CustomerInfo
{
    public class CustomerInfo : ViewComponent
    {
        public IViewComponentResult Invoke(CustomerDto customer)
        {
            return View(nameof(CustomerInfo), customer);
        }

    }
}
