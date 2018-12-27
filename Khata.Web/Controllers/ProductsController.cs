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
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly SieveService _sieveService;
        public ProductsController(IUnitOfWork db, IMapper mapper, SieveService sieveService)
        {
            _db = db;
            _mapper = mapper;
            _sieveService = sieveService;
        }

        //// GET: api/Products
        //[HttpGet]
        //public async Task<IEnumerable<ProductDto>> Get()
        //{
        //    return (await _db.Products.GetAll()).Select(m =>
        //        _mapper.Map<ProductDto>(m));
        //}


        // GET: api/Products
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> Get(string searchString = "",
            int pageSize = 0,
            int pageIndex = 1)
        {
            searchString = searchString?.ToLowerInvariant();

            var filter = string.IsNullOrEmpty(searchString)
                ? (Expression<Func<Product, bool>>)(p => true)
                : p => p.Id.ToString() == searchString
                    || p.Name.ToLowerInvariant().Contains(searchString);

            var resultsCount =
                (await _db.Products.Get(filter,
                    p => p.Id,
                    1,
                    0))
               .Count();

            var products = new List<ProductDto>();

            var sieve = _sieveService.CreateNewModel(
                searchString,
                nameof(products),
                resultsCount,
                0,
                pageIndex,
                pageSize);

            (await _db.Products.Get(
                    filter,
                    p => p.Id,
                    sieve.PageIndex,
                    sieve.PageSize))
               .ForEach(c =>
                    products.Add(_mapper.Map<ProductDto>(c)));

            sieve.SentCount = products.Count();
            return products;
        }

        // GET: api/Products/5

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute]int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var product = _mapper.Map<ProductDto>(
                await _db.Products.GetById(id));
            return Ok(product);
        }

        // POST: api/Products
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var dm = _mapper.Map<Product>(model);
            _db.Products.Add(dm);
            await _db.CompleteAsync();

            return CreatedAtAction(nameof(Get),
                new { id = dm.Id },
                _mapper.Map<ProductDto>(dm));
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]ProductViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != model.Id)
                return BadRequest();

            if (!(await Exists(id)))
                return NotFound();

            var newProduct = _mapper.Map<Product>(model);
            var originalProduct = await _db.Products.GetById(newProduct.Id);
            var meta = originalProduct.Metadata.Modified(User.Identity.Name);
            originalProduct.SetValuesFrom(newProduct);
            originalProduct.Metadata = meta;

            await _db.CompleteAsync();

            return Ok(_mapper.Map<ProductDto>(originalProduct));
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id))
            || await _db.Products.IsRemoved(id))
                return NotFound();

            await _db.Products.Remove(id);
            await _db.CompleteAsync();

            return Ok(_mapper.Map<ProductDto>(await _db.Products.GetById(id)));
        }

        // DELETE: api/Products/Permanent/5
        [HttpDelete("Permanent/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!(await Exists(id)))
                return NotFound();

            var dto = _mapper.Map<ProductDto>(await _db.Products.GetById(id));
            await _db.Products.Delete(id);
            await _db.CompleteAsync();

            return Ok(dto);
        }

        private async Task<bool> Exists(int id) =>
            await _db.Products.Exists(id);
    }
}