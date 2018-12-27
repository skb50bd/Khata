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

namespace WebUI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly SieveService _sieveService;

        public ServicesController(
            IUnitOfWork db,
            IMapper mapper,
            SieveService sieveService)
        {
            _db = db;
            _mapper = mapper;
            _sieveService = sieveService;
        }


        //// GET: api/Services
        //[HttpGet]
        //public async Task<IEnumerable<ServiceDto>> Get()
        //{
        //    return (await _db.Services.GetAll()).Select(m =>
        //        _mapper.Map<ServiceDto>(m));
        //}

        // GET: api/Services
        [HttpGet]
        public async Task<IEnumerable<ServiceDto>> Get(
            string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
        {
            searchString = searchString?.ToLowerInvariant();

            var filter = string.IsNullOrEmpty(searchString)
                ? (Expression<Func<Service, bool>>)(p => true)
                : p => p.Id.ToString() == searchString
                    || p.Name.ToLowerInvariant().Contains(searchString);

            var resultsCount =
                (await _db.Services.Get(filter,
                    p => p.Id,
                    1,
                    0))
               .Count();

            var services = new List<ServiceDto>();

            var sieve = _sieveService.CreateNewModel(
                searchString,
                nameof(services),
                resultsCount,
                0,
                pageIndex,
                pageSize);

            (await _db.Services.Get(
                    filter,
                    p => p.Id,
                    sieve.PageIndex,
                    sieve.PageSize))
               .ForEach(c =>
                    services.Add(_mapper.Map<ServiceDto>(c)));

            sieve.SentCount = services.Count();
            return services;
        }

        // GET: api/Services/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var service = _mapper.Map<ServiceDto>(
                await _db.Services.GetById(id));
            return Ok(service);
        }

        // POST: api/Services
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ServiceViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dm = _mapper.Map<Service>(model);
            _db.Services.Add(dm);
            await _db.CompleteAsync();

            return CreatedAtAction(nameof(Get),
                new { id = dm.Id },
                _mapper.Map<ServiceDto>(dm));
        }

        // PUT: api/Services/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]ServiceViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != model.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var newService = _mapper.Map<Service>(model);
            var originalService = await _db.Services.GetById(newService.Id);
            var meta = originalService.Metadata.Modified(User.Identity.Name);
            originalService.SetValuesFrom(newService);
            originalService.Metadata = meta;

            await _db.CompleteAsync();

            return Ok(_mapper.Map<ServiceDto>(originalService));
        }

        // DELETE: api/Services/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id))
            || await _db.Services.IsRemoved(id))
                return NotFound();

            await _db.Services.Remove(id);
            await _db.CompleteAsync();

            return Ok(_mapper.Map<ServiceDto>(await _db.Services.GetById(id)));
        }

        // DELETE: api/Services/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = _mapper.Map<ServiceDto>(await _db.Services.GetById(id));
            await _db.Services.Delete(id);
            await _db.CompleteAsync();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _db.Services.Exists(id);
    }
}