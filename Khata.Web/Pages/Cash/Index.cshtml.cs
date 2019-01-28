using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Domain;
using Khata.Services.CRUD;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Cash
{
    public class IndexModel : PageModel
    {
        private ICashRegisterService _cashRegister;
        private ITransactionsService _transactions;
        public IndexModel(ICashRegisterService cashRegister,
            ITransactionsService trasactions)
        {
            _cashRegister = cashRegister;
            _transactions = trasactions;
        }

        public CashRegister Cash { get; set; }

        public IEnumerable<Deposit> Despsits { get; set; }
        public IEnumerable<Withdrawal> Withdrawals { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Cash = await _cashRegister.Get();
            Despsits = await _transactions.GetDeposits(DateTime.Now.AddDays(-7), DateTime.Now);
            Withdrawals = await _transactions.GetWithdrawals(DateTime.Now.AddDays(-7), DateTime.Now);
            return Page();
        }
    }
}