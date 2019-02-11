using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Domain;
using Khata.Services.CRUD;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

using WebUI.Hubs;

namespace WebUI.Pages.Cash
{
    public class IndexModel : PageModel
    {
        private readonly ICashRegisterService _cashRegister;
        private readonly ITransactionsService _transactions;

        private readonly IHubContext<ReportsHub> _reportsHub;
        public IndexModel(ICashRegisterService cashRegister,
            ITransactionsService trasactions,
            IHubContext<ReportsHub> reportsHub)
        {
            _cashRegister = cashRegister;
            _transactions = trasactions;
            _reportsHub = reportsHub;
        }

        public CashRegister Cash { get; set; } = new CashRegister();

        public IEnumerable<Deposit> Despsits { get; set; }
        public IEnumerable<Withdrawal> Withdrawals { get; set; }

        [BindProperty]
        public DepositViewModel NewDeposit { get; set; } = new DepositViewModel();

        [BindProperty]
        public WithdrawalViewModel NewWithdrawal { get; set; } = new WithdrawalViewModel();

        public async Task<IActionResult> OnGetAsync()
        {
            Cash = await _cashRegister.Get();
            Despsits = await _transactions.GetDeposits(DateTime.Now.AddDays(-7), DateTime.Now);
            Withdrawals = await _transactions.GetWithdrawals(DateTime.Now.AddDays(-7), DateTime.Now);
            return Page();
        }

        public async Task<IActionResult> OnPostDepositAsync()
        {
            if (NewDeposit.Amount <= 0
                || NewDeposit.TableName != "Deposit")
            {
                return Page();
            }

            var deposit = await _transactions.Add(NewDeposit);
            await _reportsHub.Clients.All.SendAsync("RefreshData");
            return RedirectToPage("");
        }

        public async Task<IActionResult> OnPostWithdrawalAsync()
        {
            if (NewWithdrawal.Amount <= 0
                || NewWithdrawal.TableName != "Withdrawal")
            {
                return Page();
            }

            var withdrawal = await _transactions.Add(NewWithdrawal);
            await _reportsHub.Clients.All.SendAsync("RefreshData");
            return RedirectToPage("");
        }
    }
}