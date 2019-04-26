using System.Threading.Tasks;

using Business.Abstractions;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ViewModels;

namespace WebUI.Areas.Outgoing.Pages.Expenses
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IExpenseService _expenses;

        public CreateModel(IExpenseService expenses)
        {
            _expenses = expenses;
        }

        public IActionResult OnGet()
        {
            ExpenseVm = new ExpenseViewModel();
            return Page();
        }

        [BindProperty]
        public ExpenseViewModel ExpenseVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var expense = await _expenses.Add(ExpenseVm);

            Message = $"Expense: {expense.Id} - {expense.Name} created!";
            MessageType = "success";


            return RedirectToPage("./Index");
        }
    }
}
