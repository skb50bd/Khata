using System.Collections.Generic;
using System.Threading.Tasks;

using DTOs;
using Business.CRUD;
using Business.PageFilterSort;
using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpensesController : ControllerBase
    {
        private readonly IExpenseService _expenses;
        private readonly PfService _pfService;

        public ExpensesController(IExpenseService expenses, PfService pfService)
        {
            _expenses = expenses;
            _pfService = pfService;
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<IEnumerable<ExpenseDto>> Get(string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _expenses.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Expenses/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _expenses.Get(id));
        }

        // POST: api/Expenses
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ExpenseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _expenses.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/Expenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]ExpenseViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _expenses.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _expenses.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Expenses/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _expenses.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _expenses.Exists(id);
    }
}