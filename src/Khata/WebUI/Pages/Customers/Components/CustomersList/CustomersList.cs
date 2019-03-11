using System.Collections.Generic;

using DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Customers.Components.CustomersList
{
    public class CustomersList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<CustomerDto> customers)
        {
            return View(nameof(CustomersList), customers);
        }
    }
}
