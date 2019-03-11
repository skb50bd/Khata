﻿using System.Threading.Tasks;

using DTOs;
using Business.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.SupplierPayments
{
    public class DetailsModel : PageModel
    {
        private readonly ISupplierPaymentService _supplierPayments;
        public DetailsModel(ISupplierPaymentService supplierPayments)
        {
            _supplierPayments = supplierPayments;
        }

        public SupplierPaymentDto SupplierPayment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SupplierPayment = await _supplierPayments.Get((int)id);

            if (SupplierPayment == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}