using System.Collections.Generic;
using System.Threading.Tasks;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ViewModels;

namespace WebUI.Controllers;

[Authorize(Policy = "AdminRights")]
[Route("api/[controller]")]
[ApiController]
public class SalaryIssuesController : ControllerBase
{
    private readonly ISalaryIssueService _salaryIssues;
    private readonly PfService _pfService;

    public SalaryIssuesController(ISalaryIssueService salaryIssues, PfService pfService)
    {
        _salaryIssues = salaryIssues;
        _pfService = pfService;
    }

    // GET: api/SalaryIssues
    [HttpGet]
    public async Task<IEnumerable<SalaryIssueDto>> Get(string searchString = "",
        int pageSize = 0,
        int pageIndex = 1)
        => await _salaryIssues.Get(
            _pfService.CreateNewPf(
                searchString, pageIndex, pageSize));

    // GET: api/SalaryIssues/5

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute]int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!(await Exists(id)))
            return NotFound();

        return Ok(await _salaryIssues.Get(id));
    }

    // POST: api/SalaryIssues
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] SalaryIssueViewModel model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = await _salaryIssues.Add(model);

        if (dto == null)
            return BadRequest();

        return CreatedAtAction(nameof(Get),
            new { id = dto.Id },
            dto);
    }

    // PUT: api/SalaryIssues/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put([FromRoute]int id, [FromBody]SalaryIssueViewModel vm)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (id != vm.Id)
            return BadRequest();

        if (!(await Exists(id)))
            return NotFound();

        var dto = await _salaryIssues.Update(vm);

        if (dto == null)
            return BadRequest();

        return Ok(dto);
    }

    // DELETE: api/SalaryIssues/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Remove(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var dto = await _salaryIssues.Remove(id);

        if (dto == null)
            return BadRequest();

        return Ok(dto);
    }

    // DELETE: api/SalaryIssues/Permanent/5
    [HttpDelete("Permanent/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!(await Exists(id)))
            return NotFound();

        var dto = await _salaryIssues.Delete(id);

        if (dto == null)
            return BadRequest();

        return Ok(dto);
    }

    private async Task<bool> Exists(int id) =>
        await _salaryIssues.Exists(id);
}