﻿using Khata.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Services.Components.ServiceForm
{
    public class ServiceForm : ViewComponent
    {
        public IViewComponentResult Invoke(ServiceViewModel service)
        {
            return View("Default", service);
        }
    }
}