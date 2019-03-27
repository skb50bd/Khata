using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Abstractions;
using DTOs;
using Business.PageFilterSort;
using ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employees;
        private readonly PfService _pfService;

        public EmployeesController(PfService pfService, IEmployeeService employees)
        {
            _pfService = pfService;
            _employees = employees;
        }

        [HttpGet]
        public async Task<IEnumerable<EmployeeDto>> Get(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _employees.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _employees.Get(id));
        }

        // GET: api/Employees/Ids
        [HttpGet("Ids/")]
        public async Task<IActionResult> GetIds([FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var employees = await Get(searchString: term);
            return Ok(employees.Select(c => new { label = c.Id + " - " + c.FullName, id = c.Id }));
        }

        // POST: api/Employees
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] EmployeeViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _employees.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/Employees/5

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]EmployeeViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _employees.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _employees.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Employees/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _employees.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _employees.Exists(id);
    }
}
