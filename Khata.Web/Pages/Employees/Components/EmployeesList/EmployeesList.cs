using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Employees.Components.EmployeesList
{
    public class EmployeesList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<EmployeeDto> employees)
        {
            return View(nameof(EmployeesList), employees);
        }
    }
}
