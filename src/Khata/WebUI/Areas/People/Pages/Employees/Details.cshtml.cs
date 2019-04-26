using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.People.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly IEmployeeService _employees;
        public DetailsModel(IEmployeeService employees)
        {
            _employees = employees;
        }

        public EmployeeDto Employee { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Employee = await _employees.Get((int)id);

            if (Employee is null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
