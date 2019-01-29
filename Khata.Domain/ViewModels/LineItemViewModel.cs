
using Khata.Domain;

namespace Khata.ViewModels
{
    public class LineItemViewModel
    {
        public decimal Quantity { get; set; }
        public decimal NetPrice { get; set; }

        public int ItemId { get; set; }

        public LineItemType Type { get; set; }
    }
}