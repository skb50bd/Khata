using System.Threading.Tasks;

using AutoMapper;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Pages.Employees
{
    public class EditModel : PageModel
    {
        private readonly IEmployeeService _employees;
        private readonly IMapper _mapper;

        public EditModel(IEmployeeService employees, IMapper mapper)
        {
            _employees = employees;
            _mapper = mapper;
        }

        [BindProperty]
        public EmployeeViewModel EmployeeVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employees.Get((int)id);

            if (employee == null)
            {
                return NotFound();
            }

            EmployeeVm = _mapper.Map<EmployeeViewModel>(employee);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            EmployeeDto employee;
            try
            {
                employee = await _employees.Update(EmployeeVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await EmployeeExists((int)EmployeeVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Employee: {employee.Id} - {employee.FullName} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> EmployeeExists(int id)
        {
            return await _employees.Exists(id);
        }
    }
}
