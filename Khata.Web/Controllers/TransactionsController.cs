using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Domain;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionsService _transactions;
        private readonly PfService _pfService;

        public TransactionsController(ITransactionsService transactions, PfService pfService)
        {
            _transactions = transactions;
            _pfService = pfService;
        }

        // GET: api/Transactions/Deposits
        [HttpGet("Deposits")]
        public async Task<IEnumerable<Deposit>> GetDeposits(string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _transactions.GetDeposits(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Transactions/Withdrawals
        [HttpGet("Withdrawals")]
        public async Task<IEnumerable<Withdrawal>> GetWithdrawals(string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _transactions.GetWithdrawals(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Transactions/Deposits/5
        [HttpGet("Deposits/{id}")]
        public async Task<IActionResult> GetDepositById([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await DepositExists(id)))
                return NotFound();

            return Ok(await _transactions.GetDepositById(id));
        }

        // GET: api/Transactions/Withdrawals/5
        [HttpGet("Withdrawals/{id}")]
        public async Task<IActionResult> GetWithdrawalById([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await WithdrawalExists(id)))
                return NotFound();

            return Ok(await _transactions.GetWithdrawalById(id));
        }

        // POST: api/Transactions/Deposits
        [HttpPost("Deposits")]
        public async Task<IActionResult> Post([FromBody] DepositViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var d = await _transactions.Add(model);

            if (d == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetDepositById),
                new { id = d.Id },
                d);
        }

        // POST: api/Transactions/Withdrawals
        [HttpPost("Withdrawals")]
        public async Task<IActionResult> Post([FromBody] WithdrawalViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var w = await _transactions.Add(model);

            if (w == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetWithdrawalById),
                new { id = w.Id },
                w);
        }

        // DELETE: api/Transactions/Deposits/5
        [HttpDelete("Deposits/{id}")]
        public async Task<IActionResult> DeleteDeposit(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var d = await _transactions.DeleteDeposit(id);

            if (d == null)
                return BadRequest();

            return Ok(d);
        }

        // DELETE: api/Transactions/Withdrawals/5
        [HttpDelete("Withdrawals/{id}")]
        public async Task<IActionResult> DeleteWithdrawal(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var w = await _transactions.DeleteWithdrawal(id);

            if (w == null)
                return BadRequest();

            return Ok(w);
        }

        private async Task<bool> DepositExists(int id) =>
            await _transactions.DepositExists(id);

        private async Task<bool> WithdrawalExists(int id) =>
            await _transactions.WithdrawalExists(id);
    }
}