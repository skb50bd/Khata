using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Outgoing.Pages.Expenses
{
    public class DetailsModel : PageModel
    {
        private readonly IExpenseService _expenses;
        public DetailsModel(IExpenseService expenses)
        {
            _expenses = expenses;
        }

        public ExpenseDto Expense { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Expense = await _expenses.Get((int)id);

            if (Expense == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
