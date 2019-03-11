using DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Employees.Components.EmployeeInfo
{
    public class EmployeeInfo : ViewComponent
    {
        public IViewComponentResult Invoke(EmployeeDto employee)
        {
            return View(nameof(EmployeeInfo), employee);
        }

    }
}
