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
    public class SalaryPaymentsController : ControllerBase
    {
        private readonly ISalaryPaymentService _salaryPayments;
        private readonly PfService _pfService;

        public SalaryPaymentsController(ISalaryPaymentService salaryPayments, PfService pfService)
        {
            _salaryPayments = salaryPayments;
            _pfService = pfService;
        }

        // GET: api/SalaryPayments
        [HttpGet]
        public async Task<IEnumerable<SalaryPaymentDto>> Get(string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _salaryPayments.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/SalaryPayments/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _salaryPayments.Get(id));
        }

        // POST: api/SalaryPayments
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SalaryPaymentViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _salaryPayments.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/SalaryPayments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]SalaryPaymentViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _salaryPayments.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/SalaryPayments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _salaryPayments.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/SalaryPayments/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _salaryPayments.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _salaryPayments.Exists(id);
    }
}