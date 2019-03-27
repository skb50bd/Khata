﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Abstractions;
using DTOs;
using Business.PageFilterSort;

using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly ICustomerInvoiceService _invoices;
        private readonly PfService _pfService;

        public InvoicesController(ICustomerInvoiceService invoices,
            PfService pfService)
        {
            _invoices = invoices;
            _pfService = pfService;
        }

        // GET: api/Invoices
        [HttpGet]
        public async Task<IEnumerable<CustomerInvoiceDto>> Get(string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
            => await _invoices.Get(
                _pfService.CreateNewPf(
                    searchString, pageIndex, pageSize));

        // GET: api/Invoices/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            return Ok(await _invoices.Get(id));
        }


        // DELETE: api/Invoices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dto = await _invoices.Remove(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        // DELETE: api/Invoices/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = await _invoices.Delete(id);

            if (dto == null)
                return BadRequest();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _invoices.Exists(id);
    }
}