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

namespace WebUI.Pages.Services
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
            Services = new List<ServiceDto>();
        }

        public IList<ServiceDto> Services { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        public Sieve Sieve { get; set; }


        public async Task<IActionResult> OnGetAsync(
            string searchString = null,
            int pageSize = 40,
            int pageIndex = 1)
        {
            var filter = string.IsNullOrEmpty(searchString)
                ? (Expression<Func<Service, bool>>)(s => true)
                : (s => s.Name.ToLower()
                         .Contains(
                              searchString.ToLower()));

            var resultsCount =
                (await _db.Services
                          .Get(filter,
                               s => s.Id,
                               1,
                               0))
               .Count();

            Sieve = _sieveService.CreateNewModel(
                searchString,
                nameof(Services),
                resultsCount,
                0,
                pageIndex,
                pageSize);

            (await _db.Services.Get(
                    filter,
                    p => p.Id,
                    Sieve.PageIndex,
                    Sieve.PageSize))
                .ForEach(s =>
                    Services.Add(
                        _mapper.Map<ServiceDto>(s)));

            Sieve.SentCount = Services.Count();
            return Page();
        }
    }
}
