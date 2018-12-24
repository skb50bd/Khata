using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Khata.DTOs;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace _4._Web.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly Khata.Data.KhataContext _context;
        private readonly IMapper _mapper;
        public IndexModel(Khata.Data.KhataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            Products = new List<ProductDto>();
        }

        public IList<ProductDto> Products { get; set; }


        public string CurrentFilter { get; set; }
        public int TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(((double)TotalRecords / PageSize));
        public int PageIndex { get; set; }
        public bool HasPage(int pageNumber) => pageNumber <= TotalPages;
        public bool HasPrevPage => PageIndex > 1;
        public bool HasNextPage => HasPage(PageIndex + 1);
        public int RecordsHere => Products.Count();

        public async Task OnGetAsync(
            string searchString = null,
            int pageSize = 40,
            int pageIndex = 1)
        {
            var products = _context.Products.AsNoTracking();

            CurrentFilter = searchString;

            if (!string.IsNullOrEmpty(CurrentFilter))
                products = products.Where(p => p.Name.ToLower().Contains(CurrentFilter));

            TotalRecords = await products.CountAsync();
            PageIndex = pageIndex;
            PageSize = pageSize;

            if (PageSize > 0 && PageIndex > 0)
            {
                products = products
                    .Skip(PageSize * (PageIndex - 1))
                    .Take(PageSize);
            }

            (await products.ToListAsync())
                .ForEach(p => Products
                    .Add(_mapper.Map<ProductDto>(p)));
        }
    }
}
