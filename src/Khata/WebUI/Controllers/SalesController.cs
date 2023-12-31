﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Brotal.Extensions;

using Business.Abstractions;
using Business.PageFilterSort;

using DTOs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using ViewModels;

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
        [Authorize(Policy = "AdminRights")]
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
        [Authorize(Policy = "AdminRights")]
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
        [Authorize(Policy = "AdminRights")]
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
        [Authorize(Policy = "UserRights")]
        public async Task<IActionResult> GetLineItems(
            [FromQuery]int outletId,
            [FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IList<object> results = new List<object>();
            IEnumerable<ProductDto> products = await _products.Get(
                outletId,
                _pfService.CreateNewPf(term, 1, 100)
            );

            var emptyProducts = products.Where(p => p.InventoryTotalStock == 0);
            products = products.Except(emptyProducts).Union(emptyProducts);
            products = products.OrderBy(p => p.Name);

            var services = await _services.Get(
                outletId,
                _pfService.CreateNewPf(term, 1, 50)
            );

            products.ForEach(p => results.Add(new
            {
                Name = p.Id.ToString().PadLeft(4, '0') + " - " + p.Name,
                Available = p.InventoryTotalStock,
                UnitPriceRetail = p.PriceRetail,
                UnitPriceBulk = p.PriceBulk,
                MinimumPrice = p.PriceMargin,
                ItemId = p.Id,
                Category = "Product"
            }));

            services.ForEach(s => results.Add(new
            {
                Name = s.Id.ToString().PadLeft(4, '0') + " - " + s.Name,
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
        [Authorize(Policy = "AdminRights")]
        public async Task<IActionResult> Post(
            [FromBody] SaleViewModel model)
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
        [Authorize(Policy = "AdminRights")]
        public async Task<IActionResult> Put(
            [FromRoute]int id,
            [FromBody]SaleViewModel vm)
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
        [Authorize(Policy = "AdminRights")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _sales.Remove(id);

            if (dto == null)
                return BadRequest();

            dto.Outlet = null;

            return Ok(dto);
        }

        // DELETE: api/Sales/Permanent/5
        [HttpDelete("Permanent/{id}")]
        [Authorize(Policy = "AdminRights")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _sales.Delete(id);

            if (dto == null)
                return BadRequest();

            dto.Outlet = null;

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _sales.Exists(id);

        // POST: api/Sales/Saved
        [HttpPost("Saved/")]
        [Authorize(Policy = "AdminRights")]
        public async Task<IActionResult> PostSaved(
                    [FromBody] SaleViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _sales.Save(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetSaved),
                new { id = dto.Id },
                dto);
        }

        // GET: api/Sales/Saved
        [HttpGet("Saved/")]
        [Authorize(Policy = "AdminRights")]
        public async Task<IEnumerable<SaleDto>> GetSaved()
            => await _sales.GetSaved();

        // GET: api/Sales/Saved/5
        [HttpGet("Saved/{id}")]
        [Authorize(Policy = "AdminRights")]
        public async Task<IActionResult> GetSaved([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _sales.GetSaved(id));
        }

        // DELETE: api/Sales/Saved/5
        [HttpDelete("Saved/{id}")]
        [Authorize(Policy = "AdminRights")]
        public async Task<IActionResult> DeleteSaved(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _sales.DeleteSaved(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }
    }
}