using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstractions;
using DTOs;
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucharsController : ControllerBase
    {
        private readonly IVoucharService _vouchars;
        private readonly PfService _pfService;

        public VoucharsController(IVoucharService vouchars,
            PfService pfService)
        {
            _vouchars = vouchars;
            _pfService = pfService;
        }

        // GET: api/Vouchars
        [HttpGet]
        public async Task<IEnumerable<VoucharDto>> Get(string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _vouchars.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Vouchars/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _vouchars.Get(id));
        }


        // DELETE: api/Vouchars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _vouchars.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Vouchars/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _vouchars.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _vouchars.Exists(id);
    }
}