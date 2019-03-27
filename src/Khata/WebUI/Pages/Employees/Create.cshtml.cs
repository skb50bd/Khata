using System.Threading.Tasks;
using Business.Abstractions;
using ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Employees
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IEmployeeService _employees;

        public CreateModel(IEmployeeService employees)
        {
            _employees = employees;
        }

        public IActionResult OnGet()
        {
            EmployeeVm = new EmployeeViewModel();
            return Page();
        }

        [BindProperty]
        public EmployeeViewModel EmployeeVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var employee = await _employees.Add(EmployeeVm);

            Message = $"Employee: {employee.Id} - {employee.FullName} created!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }
    }
}