using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Employees.Components.EmployeeForm
{
    public class EmployeeForm : ViewComponent
    {
        public IViewComponentResult Invoke(EmployeeViewModel employee)
        {
            return View(nameof(EmployeeForm), employee);
        }
    }
}
