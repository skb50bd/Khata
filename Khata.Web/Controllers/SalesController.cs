using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;

using Brotal.Extensions;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _sales;
        private readonly IProductService _products;
        private readonly IServiceService _services;
        private readonly PfService _pfService;

        public SalesController(ISaleService sales,
            IProductService products,
            IServiceService services,
            PfService pfService)
        {
            _sales = sales;
            _products = products;
            _services = services;
            _pfService = pfService;
        }

        // GET: api/Sales
        [HttpGet]
        public async Task<IEnumerable<SaleDto>> Get(
            int? outletId,
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _sales.Get(
                outletId ?? 0,
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _sales.Get(id));
        }


        // GET: api/Sales/Ids
        [HttpGet("Ids/")]
        public async Task<IActionResult> GetIds([FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var customers = await Get(0, searchString: term, 50, 1);
            return Ok(customers.Select(s =>
                new
                {
                    label = $"{s.Id}({s.InvoiceId}) - {s.Customer.FullName}",
                    id = s.Id
                }));
        }


        // GET: api/Sales/LineItems
        [HttpGet("LineItems/")]
        public async Task<IActionResult> GetLineItems(
            [FromQuery]int outletId, 
            [FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IList<object> results = new List<object>();
            var products = await _products.Get(
                outletId, 
                _pfService.CreateNewPf(term, 1, 50)
            );
            var services = await _services.Get(
                outletId, 
                _pfService.CreateNewPf(term, 1, 50)
            );

            products.ForEach(p => results.Add(new
            {
                Name = p.Name,
                Available = p.InventoryTotalStock,
                UnitPriceRetail = p.PriceRetail,
                UnitPriceBulk = p.PriceBulk,
                MinimumPrice = p.PriceMargin,
                ItemId = p.Id,
                Category = "Product"
            }));
            services.ForEach(s => results.Add(new
            {
                Name = s.Name,
                Available = -1,
                UnitPriceRetail = s.Price,
                UnitPriceBulk = s.Price,
                MinimumPrice = 0,
                ItemId = s.Id,
                Category = "Service"
            }));

            return Ok(results);
        }

        // POST: api/Sales
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SaleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _sales.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/Sales/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]SaleViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _sales.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _sales.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Sales/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _sales.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _sales.Exists(id);
    }
}