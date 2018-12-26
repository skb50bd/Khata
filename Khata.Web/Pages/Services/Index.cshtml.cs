using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;

using Khata.Data.Core;
using Khata.Domain;
using Khata.DTOs;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using StonedExtensions;

namespace WebUI.Pages.Services
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _db;
        private readonly IMapper _mapper;
        public IndexModel(IUnitOfWork db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
            Services = new List<ServiceDto>();
        }

        public IList<ServiceDto> Services { get; set; }

        #region TempData
        [TempData]
        public string Message { get; set; }

        [TempData]
        public string MessageType { get; set; }
        #endregion

        #region Paging & Filtering
        public string CurrentFilter { get; set; }
        public int ResultsCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int SentCount => Services.Count();
        public int TotalPages => (int)Math.Ceiling((double)ResultsCount / PageSize);
        #endregion

        public async Task<IActionResult> OnGetAsync(
            string searchString = null,
            int pageSize = 40,
            int pageIndex = 1)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            CurrentFilter = searchString;

            var filter = string.IsNullOrEmpty(CurrentFilter)
                ? (Expression<Func<Service, bool>>)(p => true)
                : (p => p.Name.ToLower().Contains(CurrentFilter));

            ResultsCount = (await _db.Services.Get(filter, p => p.Id, 1, 0)).Count();

            (await _db.Services.Get(filter, p => p.Id, PageIndex, PageSize))
                .ForEach(s => Services.Add(_mapper.Map<ServiceDto>(s)));

            var c = Services.Count();
            return Page();
        }
    }
}
