using System.Threading.Tasks;

using Khata.Domain;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Withdrawals
{
    public class DetailsModel : PageModel
    {
        public DetailsModel(ITransactionsService transactions)
        {
            Transactions = transactions;
        }

        private ITransactionsService Transactions { get; }
        public Withdrawal Withdrawal { get; set; }

        public async Task<IActionResult> OnGetAsync([FromQuery]int? id)
        {
            if (id is null)
            {
                return NotFound();
            }

            Withdrawal = await Transactions.GetWithdrawalById((int)id);

            if (Withdrawal is null)
                return NotFound();

            return Page();
        }
    }
}