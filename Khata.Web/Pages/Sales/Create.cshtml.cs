using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Khata.Domain;
using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Pages.Sales
{
    [Authorize(Policy = "UserRights")]
    public class CreateModel : PageModel
    {
        private readonly ISaleService _sales;
        private readonly IOutletService _outlets;

        public CreateModel(
            ISaleService sales,
            IOutletService outlets)
        {
            SaleVm = new SaleViewModel();
            _sales = sales;
            _outlets = outlets;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            ViewData["Outlets"] =
                new SelectList(
                    await _outlets.Get(),
                    nameof(Outlet.Id),
                    nameof(Outlet.Title)
                );
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