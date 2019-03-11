using System.Collections.Generic;
using System.Threading.Tasks;

using DTOs;
using Business.CRUD;
using Business.PageFilterSort;
using ViewModels;

using Microsoft.AspNetCore.Mvc;

using Brotal.Extensions;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly IPurchaseService _purchases;
        private readonly IProductService _products;
        private readonly PfService _pfService;

        public PurchasesController(IPurchaseService purchases,
            IProductService products,
            PfService pfService)
        {
            _purchases = purchases;
            _products = products;
            _pfService = pfService;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<IEnumerable<PurchaseDto>> Get(string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _purchases.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Purchases/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _purchases.Get(id));
        }

        // GET: api/Purchases/LineItems
        [HttpGet("LineItems/")]
        public async Task<IActionResult> GetLineItems([FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IList<object> results = new List<object>();
            var products = await _products.Get(0, _pfService.CreateNewPf(term, 1, 0));

            products.ForEach(p => results.Add(new
            {
                Name = p.Id.ToString().PadLeft(4, '0') + "-" + p.Name,
                Available = p.InventoryTotalStock,
                UnitPurchasePrice = p.PricePurchase,
                ItemId = p.Id,
                Category = "Product"
            }));

            return Ok(results);
        }
        
        // POST: api/Purchases
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] PurchaseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _purchases.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/Purchases/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]PurchaseViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _purchases.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _purchases.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Purchases/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _purchases.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _purchases.Exists(id);
    }
}