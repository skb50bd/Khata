using System.Threading.Tasks;

using Khata.Data.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.API
{
    [Route("api/[controller]/{id:int}")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        public ServicesController(IUnitOfWork db)
        {
            _db = db;
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            if (!await _db.Services.Exists(id))
                return NotFound();
            await _db.Services.Remove(id);
            await _db.CompleteAsync();
            return Ok();
        }
    }
}