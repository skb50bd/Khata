using System.Threading.Tasks;

using Business.Abstractions;

using Domain;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Areas.Incoming.Deposits
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