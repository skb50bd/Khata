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
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customers;
        private readonly PfService _pfService;

        public CustomersController(PfService pfService, ICustomerService customers)
        {
            _pfService = pfService;
            _customers = customers;
        }

        [HttpGet]
        public async Task<IEnumerable<CustomerDto>> Get(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _customers.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _customers.Get(id));
        }

        // GET: api/Customers/Ids
        [HttpGet("Ids/")]
        public async Task<IActionResult> GetIds([FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var customers = await Get(searchString: term);
            return Ok(customers.Select(c => new { label = c.Id + " - " + c.FullName, id = c.Id }));
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _customers.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/Customers/5

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]CustomerViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _customers.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _customers.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Customers/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _customers.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _customers.Exists(id);
    }
}
