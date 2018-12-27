using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Services.Components.ServicesList
{
    public class ServicesList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<ServiceDto> services)
        {
            return View(nameof(ServicesList), services);
        }
    }
}
