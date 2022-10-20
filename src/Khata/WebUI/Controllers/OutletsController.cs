using System.Collections.Generic;
using System.Threading.Tasks;

using Business.Abstractions;

using DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ViewModels;

namespace WebUI.Controllers;

[Authorize(Policy = "AdminRights")]
[Route("api/[controller]")]
[ApiController]
public class OutletsController : ControllerBase
{
    private readonly IOutletService _outlets;

    public OutletsController(IOutletService outlets)
    {
        _outlets = outlets;
    }

    // GET: api/Outlets
    [HttpGet]
    public async Task<IEnumerable<OutletDto>> Get()
        => await _outlets.Get();

    // GET: api/Outlets/5

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute]int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!(await Exists(id)))
            return NotFound();

        return Ok(await _outlets.Get(id));
    }

    // POST: api/Outlets
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] OutletViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = await _outlets.Add(model);

        if (dto == null)
            return BadRequest();

        return CreatedAtAction(nameof(Get),
            new { id = dto.Id },
            dto);
    }

    // PUT: api/Outlets/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute]int id, [FromBody]OutletViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != vm.Id)
            return BadRequest();

        if (!(await Exists(id)))
            return NotFound();

        var dto = await _outlets.Update(vm);

        if (dto == null)
            return BadRequest();

        return Ok(dto);
    }

    // DELETE: api/Outlets/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState); 

        var dto = await _outlets.Remove(id);

        if (dto == null)
            return BadRequest();

        dto.Sales = null;
        dto.Products = null;
        dto.Services = null;

        return Ok(dto);
    }

    // DELETE: api/Outlets/Permanent/5
    [HttpDelete("Permanent/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!(await Exists(id)))
            return NotFound();

        var dto = await _outlets.Delete(id);

        if (dto == null)
            return BadRequest();

        dto.Sales = null;
        dto.Products = null;
        dto.Services = null;

        return Ok(dto);
    }

    private async Task<bool> Exists(int id) =>
        await _outlets.Exists(id);
}