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
                Filter = filter?.ToLowerInvariant(),
                SearchArea = searchArea,
                ResultsCount = resultsCount,
                PageSize = pageSize,
                PageIndex = pageIndex,
                SentCount = sentCount
            };
        }

        public PageFilter CreateNewPf(string filter, int index = 1, int size = 0)
        {
            if (size == 0)
            {
                size = int.MaxValue;
            }
            return new PageFilter(filter, index, size);
        }
    }
}