using System.Collections.Generic;

namespace Khata.ViewModels
{
    public class RefundViewModel
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int SaleId { get; set; }

        public ICollection<LineItemViewModel> Cart { get; set; }

        public decimal CashBack { get; set; }

        public decimal DebtRollback { get; set; }

        public string Description { get; set; }
    }
}
