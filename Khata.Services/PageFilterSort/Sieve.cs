using System;

using Microsoft.Extensions.Configuration;
namespace Khata.Services.PageFilterSort
{
    public class SieveService
    {
        private readonly IConfiguration _configuration;

        public SieveService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Sieve CreateNewModel(string filter,
            string searchArea,
            int resultsCount,
            int sentCount,
            int pageIndex = 1,
            int pageSize = 0)
        {
            if (pageSize == 0)
            {
                pageSize = _configuration.GetValue<int>("DefaultPageSize");
            }

            if (pageSize == 0)
            {
                pageSize = int.MaxValue;
            }

            return new Sieve
            {
                Filter = filter,
                SearchArea = searchArea,
                ResultsCount = resultsCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                SentCount = sentCount
            };
        }
    }

    public class Sieve
    {
        public string Filter { get; set; }
        public string SearchArea { get; set; }
        public int ResultsCount { get; set; }
        public int SentCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)ResultsCount / PageSize);
        public bool HasPage(int pageNumber) => pageNumber <= TotalPages;
        public bool HasPrevPage => PageIndex > 1;
        public bool HasNextPage => HasPage(PageIndex + 1);
    }
}