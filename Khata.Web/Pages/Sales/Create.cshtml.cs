using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Sales
{
    [Authorize(Policy = "UserRights")]
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
                    Debug.WriteLine(e.ErrorMessage);
                return Page();
            }

            SaleDto sale;
            try
            {
                sale = await _sales.Add(SaleVm);

            }
            catch (Exception e)
            {
                if (e.Message == "Invalid Operation")
                {
                    MessageType = "danger";
                    Message = "Nothing to Create";
                    return Page();
                }
                else
                    throw;
            }

            MessageType = "success";
            if (sale?.Id > 0)
            {
                Message = $"Sale: {sale.Id} - {sale.Customer.FullName} created!";
                return RedirectToPage("./Index");
            }
            else
            {
                Message = $"Debt Payment received from {sale.Customer.FullName}!";
                return RedirectToPage("../DebtPayments/Index");
            }

        }
    }
}