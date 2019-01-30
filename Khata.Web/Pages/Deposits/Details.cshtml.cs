﻿using System.Threading.Tasks;

using Khata.Domain;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Deposits
{
    public class DetailsModel : PageModel
    {
        public DetailsModel(ITransactionsService transactions)
        {
            Transactions = transactions;
        }

        private ITransactionsService Transactions { get; }
        public Deposit Deposit { get; set; }

        public async Task<IActionResult> OnGetAsync([FromQuery]int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Deposit = await Transactions.GetDepositById((int)id);

            if (Deposit is null)
                return NotFound();

            return Page();
        }
    }
}