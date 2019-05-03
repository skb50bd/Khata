using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ViewModels;

namespace WebUI.Controllers
{
    [Authorize(Policy = "AdminRights")]
    [Route("api/[controller]")]
    [ApiController]
    public class SuppliersController : ControllerBase
    {
        private readonly ISupplierService _suppliers;
        private readonly PfService _pfService;

        public SuppliersController(PfService pfService, ISupplierService suppliers)
        {
            _pfService = pfService;
            _suppliers = suppliers;
        }

        [HttpGet]
        public async Task<IEnumerable<SupplierDto>> Get(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _suppliers.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Suppliers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _suppliers.Get(id));
        }

        // GET: api/Suppliers/Ids
        [HttpGet("Ids/")]
        public async Task<IActionResult> GetIds([FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var suppliers = await Get(searchString: term);
            return Ok(suppliers.Select(c => new { label = c.Id + " - " + c.FullName, id = c.Id }));
        }

        // POST: api/Suppliers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SupplierViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _suppliers.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/Suppliers/5

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]SupplierViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _suppliers.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Suppliers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _suppliers.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Suppliers/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _suppliers.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _suppliers.Exists(id);
    }
}
