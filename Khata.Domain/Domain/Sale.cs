using System.Collections.Generic;

namespace Khata.Domain
{
    public class Sale : TrackedEntity, IDeposit
    {
        public SaleType Type { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<SaleLineItem> Cart { get; set; }
        public PaymentInfo Payment { get; set; }
        public string Description { get; set; }

        public decimal Amount => Payment.Paid;
        public string TableName => nameof(Sale);
        public int RowId => Id;
    }
}