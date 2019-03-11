using Domain;

using Microsoft.Extensions.Options;

namespace Business.PageFilterSort
{
    public class PfService
    {
        public OutletOptions Options { get; set; }

        public PfService(IOptionsMonitor<OutletOptions> monitor)
        {
            Options = monitor.CurrentValue;
        }

        public PageFilter CreateNewPf(string filter, int index = 1, int size = 0)
        {
            if (size == 0)
            {
                size = Options.DefaultPageSize;
            }
            return new PageFilter(filter, index, size);
        }
    }
}