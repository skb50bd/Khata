using System.Threading.Tasks;
using Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

[Authorize(Policy = "AdminRights")]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(
        UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    // DELETE: api/Users/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userManager.FindByIdAsync(id);
        var res = await _userManager.DeleteAsync(user);

        if (!res.Succeeded)
            return BadRequest();

        return Ok();
    }
}