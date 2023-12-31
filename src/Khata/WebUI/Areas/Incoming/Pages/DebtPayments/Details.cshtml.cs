﻿using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Incoming.Pages.DebtPayments
{
    public class DetailsModel : PageModel
    {
        private readonly IDebtPaymentService _debtPayments;
        public DetailsModel(IDebtPaymentService debtPayments)
        {
            _debtPayments = debtPayments;
        }

        public DebtPaymentDto DebtPayment { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            DebtPayment = await _debtPayments.Get((int)id);

            if (DebtPayment == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
