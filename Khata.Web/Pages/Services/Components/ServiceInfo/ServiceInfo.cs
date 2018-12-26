using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Services.Components.ServiceInfo
{
    public class ServiceInfo : ViewComponent
    {
        public IViewComponentResult Invoke(ServiceDto service)
        {
            return View("Default", service);
        }

    }
}
