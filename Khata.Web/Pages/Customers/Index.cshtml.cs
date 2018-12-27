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

using StonedExtensions;

namespace WebUI.Pages.Customers
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
            Customers = new List<CustomerDto>();
        }

        public IList<CustomerDto> Customers { get; set; }

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

            Sieve = _sieveService.CreateNewModel(
                searchString,
                nameof(Products),
                resultsCount,
                0,
                pageIndex,
                pageSize);

            (await _db.Customers.Get(
                    filter,
                    p => p.Id,
                    Sieve.PageIndex,
                    Sieve.PageSize))
               .ForEach(p =>
                    Customers.Add(
                        _mapper.Map<CustomerDto>(p)));

            Sieve.SentCount = Customers.Count();
            return Page();
        }
    }
}
