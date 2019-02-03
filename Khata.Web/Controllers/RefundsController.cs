﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Khata.Domain;
using Khata.DTOs;
using Khata.Services.CRUD;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;

using SharedLibrary;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefundsController : ControllerBase
    {
        private readonly IRefundService _refunds;
        private readonly ISaleService _sales;
        private readonly PfService _pfService;

        public RefundsController(IRefundService refunds,
            ISaleService sales,
            PfService pfService)
        {
            _refunds = refunds;
            _sales = sales;
            _pfService = pfService;
        }

        // GET: api/Refunds
        [HttpGet]
        public async Task<IEnumerable<RefundDto>> Get(string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _refunds.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Refunds/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _refunds.Get(id));
        }

        // GET: api/Refunds/Sales
        [HttpGet("Sales/")]
        public async Task<IActionResult> GetCustomerSales([FromQuery]int customerId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IList<object> results = new List<object>();
            (await _sales.GetCustomerSales(customerId))
                .ForEach(
                    s => results.Add(
                        new
                        {
                            s.Id,
                            Date = s.SaleDate,
                            Paid = s.PaymentPaid,
                            ItemsCount = s.Cart.Count
                        }
                ));

            return Ok(results);
        }


        // GET: api/Refunds/LineItems
        [HttpGet("LineItems/")]
        public async Task<IActionResult> GetLineItems([FromQuery]int saleId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await _sales.Exists(saleId)))
                return NotFound();

            IEnumerable<SaleLineItem> results =
                (await _sales.Get(saleId)).Cart
                    .Where(li => li.Type == LineItemType.Product);


            return Ok(results);
        }


        // POST: api/Refunds
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RefundViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _refunds.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/Refunds/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]RefundViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _refunds.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Refunds/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _refunds.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Refunds/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _refunds.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _refunds.Exists(id);
    }
}