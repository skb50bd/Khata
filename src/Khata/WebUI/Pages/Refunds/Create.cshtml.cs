using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstractions;
using DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using ViewModels;

namespace WebUI.Pages.Refunds
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IRefundService _refunds;

        public CreateModel(IRefundService refunds)
        {
            RefundVm = new RefundViewModel();
            _refunds = refunds;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public RefundViewModel RefundVm { get; set; }

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

            RefundDto refund;
            try
            {
                refund = await _refunds.Add(RefundVm);

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
            Message = $"Refund: {refund.Id} - {refund.Customer.FullName} created!";

            return RedirectToPage("./Index");

        }
    }
}