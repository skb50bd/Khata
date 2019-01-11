using System.Collections.Generic;

namespace Khata.Domain
{
    public class Sale : TrackedEntity
    {
        public SaleType Type { get; set; }
        public int CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<SaleLineItem> Cart { get; set; }
        public PaymentInfo Payment { get; set; }
    }
}