using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceService _services;
        private readonly SieveService _sieveService;

        public ServicesController(
            SieveService sieveService, IServiceService services)
        {
            _sieveService = sieveService;
            _services = services;
        }

        // GET: api/Services
        [HttpGet]
        public async Task<IEnumerable<ServiceDto>> Get(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _services.Get(
                _sieveService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Services/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _services.Get(id));
        }

        // POST: api/Services
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ServiceViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _services.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/Services/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]ServiceViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _services.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _services.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Services/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _services.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _services.Exists(id);
    }
}