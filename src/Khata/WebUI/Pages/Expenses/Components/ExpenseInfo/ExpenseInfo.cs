using DTOs;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Expenses.Components.ExpenseInfo
{
    public class ExpenseInfo : ViewComponent
    {
        public IViewComponentResult Invoke(ExpenseDto expense)
        {
            return View(nameof(ExpenseInfo), expense);
        }

    }
}
