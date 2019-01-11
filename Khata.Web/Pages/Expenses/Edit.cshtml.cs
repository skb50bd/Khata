using System.Threading.Tasks;

using AutoMapper;

using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Pages.Expenses
{
    public class EditModel : PageModel
    {
        private readonly IExpenseService _expenses;
        private readonly IMapper _mapper;

        public EditModel(IExpenseService expenses, IMapper mapper)
        {
            _expenses = expenses;
            _mapper = mapper;
        }

        [BindProperty]
        public ExpenseViewModel ExpenseVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _expenses.Get((int)id);

            if (expense == null)
            {
                return NotFound();
            }

            ExpenseVm = _mapper.Map<ExpenseViewModel>(expense);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                await _expenses.Update(ExpenseVm);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ExpenseExists(ExpenseVm.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            Message = $"Expense: {ExpenseVm.Id} - {ExpenseVm.Name} updated!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }

        private async Task<bool> ExpenseExists(int id)
        {
            return await _expenses.Exists(id);
        }
    }
}
