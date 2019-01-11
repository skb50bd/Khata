using System.Collections.Generic;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Expenses.Components.ExpensesList
{
    public class ExpensesList : ViewComponent
    {
        public IViewComponentResult Invoke(IEnumerable<ExpenseDto> expenses)
        {
            return View(nameof(ExpensesList), expenses);
        }
    }
}
