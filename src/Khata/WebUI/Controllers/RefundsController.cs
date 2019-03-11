using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Domain;
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
        public async Task<IEnumerable<RefundDto>> Get(
            string searchString = "",
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
        public async Task<IActionResult> GetSales(
            [FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<SaleDto> sales;
            IList<object> results = new List<object>();

            sales = await _sales.Get(0,
                        _pfService.CreateNewPf(term, 1, int.MaxValue),
                        DateTime.Today.AddYears(-1),
                        DateTime.Now);

            string getLabel(int saleId, int invoiceId, string customerName)
            {
                var sb = new StringBuilder();
                sb.Append("S");
                sb.Append(saleId.ToString().PadLeft(6, '0'));
                sb.Append(" I");
                sb.Append(invoiceId.ToString().PadLeft(6, '0'));
                sb.Append(" - ");
                sb.Append(customerName);
                return sb.ToString();
            }
            sales.ForEach(
                        s => results.Add(
                            new
                            {
                                Label = getLabel(s.Id, s.InvoiceId, s.Customer.FullName),
                                s.Id,
                                s.CustomerId,
                                Date = s.SaleDate,
                                Paid = s.PaymentPaid,
                                ItemsCount = s.Cart.Count
                            }
                    ));
            return Ok(results);
        }


        // GET: api/Refunds/LineItems/5
        [HttpGet("LineItems/{saleId:int}")]
        public async Task<IActionResult> GetLineItems(
            [FromRoute]int saleId,
            [FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<SaleLineItem> results =
                (await _sales.Get(saleId)).Cart
                    .Where(li => li.Type == LineItemType.Product
                        && (string.IsNullOrWhiteSpace(term)
                        || li.Name.ToLowerInvariant().Contains(term.ToLowerInvariant())));

            var sales = new List<object>();
            foreach (var item in results)
            {
                sales.Add(
                    new
                    {
                        Label = item.Name,
                        item.Id,
                        Value = item
                    }
                );
            }
            return Ok(sales);
        }


        // POST: api/Refunds
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] RefundViewModel model)
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
        public async Task<IActionResult> Put(
            [FromRoute]int id,
            [FromBody]RefundViewModel vm)
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