using System.Collections.Generic;
using System.Threading.Tasks;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ViewModels;

namespace WebUI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _products;
    private readonly PfService _pfService;

    public ProductsController(IProductService products, PfService pfService)
    {
        _products = products;
        _pfService = pfService;
    }

    // GET: api/Products
    [HttpGet]
    [Authorize(Policy = "AdminRights")]
    public async Task<IEnumerable<ProductDto>> Get(
        int? outletId,
        string searchString = "",
        int pageSize = 0,
        int pageIndex = 1)
        => await _products.Get(
            outletId ?? 0,
            _pfService.CreateNewPf(
                searchString, pageIndex, pageSize));

    // GET: api/Products/5

    [HttpGet("{id}")]
    [Authorize(Policy = "UserRights")]
    public async Task<IActionResult> Get([FromRoute]int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!(await Exists(id)))
            return NotFound();

        return Ok(await _products.Get(id));
    }

    // POST: api/Products
    [HttpPost]
    [Authorize(Policy = "AdminRights")]
    public async Task<IActionResult> Post([FromBody] ProductViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = await _products.Add(model);

        if (dto == null)
            return BadRequest();

        return CreatedAtAction(nameof(Get),
            new { id = dto.Id },
            dto);
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    [Authorize(Policy = "AdminRights")]
    public async Task<IActionResult> Put([FromRoute]int id, [FromBody]ProductViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != vm.Id)
            return BadRequest();

        if (!(await Exists(id)))
            return NotFound();

        var dto = await _products.Update(vm);

        if (dto == null)
            return BadRequest();

        return Ok(dto);
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    [Authorize(Policy = "AdminRights")]
    public async Task<IActionResult> Remove(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = await _products.Remove(id);
        dto.Outlet = null;

        if (dto == null)
            return BadRequest();

        dto.Outlet = null;

        return Ok(dto);
    }

    // DELETE: api/Products/Permanent/5
    [HttpDelete("Permanent/{id}")]
    [Authorize(Policy = "AdminRights")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!(await Exists(id)))
            return NotFound();

        var dto = await _products.Delete(id);

        if (dto == null)
            return BadRequest();

        dto.Outlet = null;

        return Ok(dto);
    }

    private async Task<bool> Exists(int id) =>
        await _products.Exists(id);
}