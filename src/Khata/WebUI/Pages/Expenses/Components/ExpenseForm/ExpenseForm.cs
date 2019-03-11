using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Pages.Expenses.Components.ExpenseForm
{
    public class ExpenseForm : ViewComponent
    {
        public IViewComponentResult Invoke(ExpenseViewModel expense)
        {
            return View(nameof(ExpenseForm), expense);
        }
    }
}
