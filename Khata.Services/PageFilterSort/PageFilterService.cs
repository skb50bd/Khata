using Microsoft.Extensions.Configuration;

namespace Khata.Services.PageFilterSort
{
    public class PfService
    {
        private readonly IConfiguration _configuration;

        public PfService(IConfiguration configuration)
        {
            _configuration = configuration;
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