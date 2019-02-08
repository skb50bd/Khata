﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.PurchaseReturns
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly IPurchaseReturnService _purchaseReturns;
        private readonly IPurchaseService _purchases;

        public CreateModel(
            IPurchaseReturnService purchaseReturns,
            IPurchaseService purchases)
        {
            PurchaseReturnVm = new PurchaseReturnViewModel();
            _purchaseReturns = purchaseReturns;
            _purchases = purchases;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public PurchaseReturnViewModel PurchaseReturnVm { get; set; }

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

            PurchaseReturnDto purchaseReturn;
            try
            {
                purchaseReturn = await _purchaseReturns.Add(PurchaseReturnVm);

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
            Message = $"PurchaseReturn: {purchaseReturn.Id} - {purchaseReturn.Supplier.FullName} created!";

            return RedirectToPage("./Index");

        }
    }
}