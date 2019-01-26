using System.Linq;
using System.Threading.Tasks;

using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Sales
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ISaleService _sales;

        public CreateModel(ISaleService sales)
        {
            SaleVm = new SaleViewModel();
            _sales = sales;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public SaleViewModel SaleVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                foreach (var e in ModelState.Values.SelectMany(v => v.Errors))
                {
                    System.Console.WriteLine(e.ErrorMessage);
                }

                return Page();
            }

            var sale = await _sales.Add(SaleVm);

            Message = $"Sale: {sale.Id} - {sale.Customer.FullName} created!";
            MessageType = "success";

            return RedirectToPage("./Index");
        }
    }
}