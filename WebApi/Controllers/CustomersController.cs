using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.DTOs;
using Khata.Services.PageFilterSort;
using Khata.ViewModels;

using Microsoft.AspNetCore.Mvc;

using StonedExtensions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly SieveService _sieveService;

        public CustomersController(IUnitOfWork db, IMapper mapper, SieveService sieveService)
        {
            _db = db;
            _mapper = mapper;
            _sieveService = sieveService;
        }

        //// GET: api/Customers
        //[HttpGet]
        //public async Task<IEnumerable<CustomerDto>> Get()
        //{
        //    return (await _db.Customers.GetAll()).Select(m =>
        //        _mapper.Map<CustomerDto>(m));
        //}

        // GET: api/Customers
        [HttpGet]
        public async Task<IEnumerable<CustomerDto>> Get(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
        {
            searchString = searchString?.ToLowerInvariant();

            var filter = string.IsNullOrEmpty(searchString)
                ? (Expression<Func<Customer, bool>>)(p => true)
                : p => p.Id.ToString() == searchString
                    || p.FullName.ToLowerInvariant().Contains(searchString)
                    || p.CompanyName.ToLowerInvariant().Contains(searchString)
                    || p.Phone.Contains(searchString)
                    || p.Email.Contains(searchString);

            var resultsCount =
                (await _db.Customers.Get(filter,
                    p => p.Id,
                    1,
                    0))
               .Count();

            var customers = new List<CustomerDto>();

            var sieve = _sieveService.CreateNewModel(
                searchString,
                nameof(customers),
                resultsCount,
                0,
                pageIndex,
                pageSize);

            (await _db.Customers.Get(
                    filter,
                    p => p.Id,
                    sieve.PageIndex,
                    sieve.PageSize))
               .ForEach(c =>
                    customers.Add(_mapper.Map<CustomerDto>(c)));

            sieve.SentCount = customers.Count();
            return customers;
        }

        // GET: api/Customers/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var customer = _mapper.Map<CustomerDto>(
                await _db.Customers.GetById(id));
            return Ok(customer);
        }

        // POST: api/Customers
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dm = _mapper.Map<Customer>(model);
            dm.Metadata = Metadata.CreatedNew(User.Identity.Name);
            _db.Customers.Add(dm);
            await _db.CompleteAsync();

            return CreatedAtAction(nameof(Get),
                new { id = dm.Id },
                _mapper.Map<CustomerDto>(dm));
        }

        // PUT: api/Customers/5

        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]CustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != model.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var newCustomer = _mapper.Map<Customer>(model);
            var originalCustomer = await _db.Customers.GetById(newCustomer.Id);
            var meta = originalCustomer.Metadata.Modified(User.Identity.Name);
            originalCustomer.SetValuesFrom(newCustomer);
            originalCustomer.Metadata = meta;

            await _db.CompleteAsync();

            return Ok(_mapper.Map<CustomerDto>(originalCustomer));
        }

        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id))
            || await _db.Customers.IsRemoved(id))
                return NotFound();

            await _db.Customers.Remove(id);
            await _db.CompleteAsync();

            return Ok(_mapper.Map<CustomerDto>(await _db.Customers.GetById(id)));
        }

        // DELETE: api/Customers/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = _mapper.Map<CustomerDto>(await _db.Customers.GetById(id));
            await _db.Customers.Delete(id);
            await _db.CompleteAsync();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _db.Customers.Exists(id);
    }
}
