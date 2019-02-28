using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Brotal.Extensions;

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

        public IEnumerable<Deposit> Deposits { get; set; }
        public IEnumerable<Withdrawal> Withdrawals { get; set; }

        [BindProperty]
        public DepositViewModel NewDeposit { get; set; } = new DepositViewModel();

        [BindProperty]
        public WithdrawalViewModel NewWithdrawal { get; set; } = new WithdrawalViewModel();

        [BindProperty]
        public string FromText { get; set; }
        [BindProperty]
        public string ToText { get; set; }


        public async Task<IActionResult> OnGetAsync()
        {
            Cash = await _cashRegister.Get();
            Deposits = await _transactions.GetDeposits(
                DateTime.Now.AddDays(-7),
                DateTime.Now);

            Withdrawals = await _transactions.GetWithdrawals(
                DateTime.Now.AddDays(-7),
                DateTime.Now);

            return Page();
        }

        public async Task<IActionResult> OnPostWithDateRangeAsync()
        {
            if (string.IsNullOrWhiteSpace(FromText)
                || string.IsNullOrWhiteSpace(ToText))
                return Page();
            Cash = await _cashRegister.Get();
            Deposits =
                await _transactions.GetDeposits(
                    FromText.ParseDate(),
                    ToText.ParseDate().AddMinutes(23 * 60 + 59)
                );
            Withdrawals =
                await _transactions.GetWithdrawals(
                    FromText.ParseDate(),
                    ToText.ParseDate().AddMinutes(23 * 60 + 59)
                );
            return Page();
        }

        public async Task<IActionResult> OnPostDepositAsync()
        {
            if (NewDeposit.Amount <= 0
                || NewDeposit.TableName != "Deposit"
                || string.IsNullOrWhiteSpace(NewDeposit.Description))
            {
                return Page();
            }

            _ = await _transactions.Add(NewDeposit);
            await _reportsHub.Clients.All.SendAsync("RefreshData");
            return RedirectToPage("");
        }

        public async Task<IActionResult> OnPostWithdrawalAsync()
        {
            if (NewWithdrawal.Amount <= 0
                || NewWithdrawal.TableName != "Withdrawal"
                || string.IsNullOrWhiteSpace(NewWithdrawal.Description))
            {
                return Page();
            }

            _ = await _transactions.Add(NewWithdrawal);
            await _reportsHub.Clients.All.SendAsync("RefreshData");
            return RedirectToPage("");
        }
    }
}