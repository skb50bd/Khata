using System;
using System.Threading.Tasks;
using Business.Abstractions;
using DTOs;
using ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Purchases
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IPurchaseService _purchases;

        public CreateModel(IPurchaseService purchases)
        {
            PurchaseVm = new PurchaseViewModel();
            _purchases = purchases;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PurchaseViewModel PurchaseVm { get; set; }

        [TempData] public string Message { get; set; }
        [TempData] public string MessageType { get; set; }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            PurchaseDto purchase;
            try
            {
                purchase = await _purchases.Add(PurchaseVm);

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
            if (purchase.Id > 0)
            {
                Message = $"Purchase: {purchase.Id} - {purchase.Supplier.FullName} created!";
                return RedirectToPage("./Index");
            }
            else
            {
                Message = $"Supplier Payment paid to {purchase.Supplier.FullName}!";
                return RedirectToPage("../SupplierPayments/Index");
            }

        }
    }
}