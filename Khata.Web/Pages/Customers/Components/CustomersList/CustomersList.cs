using System.Collections.Generic;
using Khata.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Shared.Components.CustomersList
{
    public class CustomersList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<CustomerDto> customers)
        {
            return View("Default", customers);
        }
    }
}
