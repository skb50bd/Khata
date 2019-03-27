using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Brotal.Extensions;
using Business.Abstractions;
using Domain;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;

using ViewModels;

using WebUI.Hubs;

namespace WebUI.Pages.Cash
{
    public class IndexModel : PageModel
    {
        private readonly ICashRegisterService _cashRegister;
        private readonly ITransactionsService _transactions;
        private readonly IHubContext<ReportsHub> _reportsHub;

        public IndexModel(
            ICashRegisterService cashRegister,
            ITransactionsService trasactions,
            IHubContext<ReportsHub> reportsHub)
        {
            _cashRegister = cashRegister;
            _transactions = trasactions;
            _reportsHub   = reportsHub;
        }

        #region Data Properties
        public CashRegister Cash { get; set; }
            = new CashRegister();

        public IEnumerable<Deposit> Deposits { get; set; }
        public IEnumerable<Withdrawal> Withdrawals { get; set; }
        #endregion

        #region Deposit-Withdrawal Form Data
        [BindProperty]
        public DepositViewModel NewDeposit { get; set; }
            = new DepositViewModel();

        [BindProperty]
        public WithdrawalViewModel NewWithdrawal { get; set; }
            = new WithdrawalViewModel();
        #endregion

        #region Date Range
        [BindProperty]
        public string FromText { get; set; }

        [BindProperty]
        public string ToText { get; set; }

        public DateTime FromDate
            => FromText.ParseDate();
        public DateTime ToDate
            => ToText.ParseDate()
                .AddMinutes(23 * 60 + 59);
        #endregion

        public async Task<IActionResult> OnGetAsync(
            string fromText, 
            string toText)
        {
            FromText = fromText ?? DateTime.Today.LocalDate();
            ToText   = toText ?? FromText;

            await Load();
            return Page();
        }

        private async Task Load()
        {
            Cash = await _cashRegister.Get();
            Deposits =
                await _transactions
                    .GetDeposits(
                        FromDate,
                        ToDate);
            Withdrawals =
                await _transactions
                    .GetWithdrawals(
                        FromDate,
                        ToDate);
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