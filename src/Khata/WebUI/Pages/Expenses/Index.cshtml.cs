using System.Threading.Tasks;

using DTOs;
using Business.CRUD;
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Brotal.Extensions;

namespace WebUI.Pages.Expenses
{
    public class IndexModel : PageModel
    {
        private readonly IExpenseService _expenses;
        private readonly PfService _pfService;
        public IndexModel(IExpenseService expenses, PfService pfService)
        {
            _expenses = expenses;
            _pfService = pfService;
            Expenses = new PagedList<ExpenseDto>();
        }

        public IPagedList<ExpenseDto> Expenses { get; set; }
        public PageFilter Pf { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        public async Task<IActionResult> OnGetAsync(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
        {
            Pf = _pfService.CreateNewPf(searchString, pageIndex, pageSize);
            Expenses = await _expenses.Get(Pf);
            return Page();
        }
    }
}
