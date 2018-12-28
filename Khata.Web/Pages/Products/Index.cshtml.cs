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

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using SharedLibrary;

namespace WebUI.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        private readonly SieveService _sieveService;
        public IndexModel(IUnitOfWork db,
            IMapper mapper,
            SieveService sieveService)
        {
            _db = db;
            _mapper = mapper;
            _sieveService = sieveService;
            Products = new List<ProductDto>();
        }

        public IList<ProductDto> Products { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        public Sieve Sieve { get; set; }

        public async Task<IActionResult> OnGetAsync(
            string searchString = null,
            int pageSize = 0,
            int pageIndex = 1)
        {
            var filter = string.IsNullOrEmpty(searchString)
                ? (Expression<Func<Product, bool>>)(p => true)
                : (p => p.Name.ToLower()
                         .Contains(
                              searchString.ToLower()));

            var resultsCount =
                (await _db.Products.Get(filter,
                          p => p.Id,
                          1,
                          0))
               .Count();

            Sieve = _sieveService.CreateNewModel(
                searchString,
                nameof(Products),
                resultsCount,
                0,
                pageIndex,
                pageSize);

            (await _db.Products.Get(
                    filter,
                    p => p.Id,
                    Sieve.PageIndex,
                    Sieve.PageSize))
               .ForEach(p =>
                    Products.Add(
                        _mapper.Map<ProductDto>(p)));

            Sieve.SentCount = Products.Count();
            return Page();
        }
    }
}
