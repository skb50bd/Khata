using System.Data;
using System.Threading.Tasks;

using Khata.Data.Core;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.API
{
    [Route("api/[controller]/{id:int}")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        public ProductsController(IUnitOfWork db)
        {
            _db = db;
        }

        [Authorize]
        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            if (!await _db.Products.Exists(id))
            {
                return NotFound();
            }
            else
            {
                try
                {
                    await _db.Products.Remove(id);
                    await _db.CompleteAsync();
                    return Ok();
                }
                catch (DBConcurrencyException)
                {
                    throw;
                }
            }
        }

    }
}