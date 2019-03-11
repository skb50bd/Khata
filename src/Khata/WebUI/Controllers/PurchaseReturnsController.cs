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
    public class PurchaseReturnsController : ControllerBase
    {
        private readonly IPurchaseReturnService _purchaseReturns;
        private readonly IPurchaseService _purchases;
        private readonly PfService _pfService;

        public PurchaseReturnsController(
            IPurchaseReturnService purchaseReturns,
            IPurchaseService purchases,
            PfService pfService)
        {
            _purchaseReturns = purchaseReturns;
            _purchases = purchases;
            _pfService = pfService;
        }

        // GET: api/PurchaseReturns
        [HttpGet]
        public async Task<IEnumerable<PurchaseReturnDto>> Get(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _purchaseReturns.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/PurchaseReturns/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _purchaseReturns.Get(id));
        }

        // GET: api/PurchaseReturns/Purchases
        [HttpGet("Purchases/")]
        public async Task<IActionResult> GetPurchases(
            [FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<PurchaseDto> purchases;
            IList<object> results = new List<object>();

            purchases = await _purchases.Get(
                        _pfService.CreateNewPf(term, 1, int.MaxValue),
                        DateTime.Today.AddYears(-1),
                        DateTime.Now);

            string getLabel(int purchaseId,
                int invoiceId,
                string supplierName)
            {
                var sb = new StringBuilder();
                sb.Append("P");
                sb.Append(purchaseId.ToString().PadLeft(6, '0'));
                sb.Append(" V");
                sb.Append(invoiceId.ToString().PadLeft(6, '0'));
                sb.Append(" - ");
                sb.Append(supplierName);
                return sb.ToString();
            }
            purchases.ForEach(
                        s => results.Add(
                            new
                            {
                                Label = getLabel(s.Id, s.VoucharId, s.Supplier.FullName),
                                s.Id,
                                s.SupplierId,
                                Date = s.PurchaseDate,
                                Paid = s.PaymentPaid,
                                ItemsCount = s.Cart.Count
                            }
                    ));
            return Ok(results);
        }


        // GET: api/PurchaseReturns/LineItems/5
        [HttpGet("LineItems/{purchaseId:int}")]
        public async Task<IActionResult> GetLineItems(
            [FromRoute]int purchaseId,
            [FromQuery]string term)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            IEnumerable<PurchaseLineItem> results =
                (await _purchases.Get(purchaseId)).Cart
                    .Where(li => string.IsNullOrWhiteSpace(term)
                                || li.Name.ToLowerInvariant()
                                    .Contains(term.ToLowerInvariant()));

            var purchaseItems = new List<object>();
            foreach (var item in results)
            {
                purchaseItems.Add(
                    new
                    {
                        Label = item.Name,
                        item.Id,
                        Value = item
                    }
                );
            }
            return Ok(purchaseItems);
        }


        // POST: api/PurchaseReturns
        [HttpPost]
        public async Task<IActionResult> Post(
            [FromBody] PurchaseReturnViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _purchaseReturns.Add(model);

            if (dto == null)
                return BadRequest();

            return CreatedAtAction(nameof(Get),
                new { id = dto.Id },
                dto);
        }

        // PUT: api/PurchaseReturns/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(
            [FromRoute]int id,
            [FromBody]PurchaseReturnViewModel vm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != vm.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _purchaseReturns.Update(vm);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/PurchaseReturns/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _purchaseReturns.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/PurchaseReturns/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _purchaseReturns.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _purchaseReturns.Exists(id);
    }
}