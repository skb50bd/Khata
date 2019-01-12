using System.Collections.Generic;

namespace Khata.Domain
{
    public class Purchase : TrackedEntity
    {
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public virtual ICollection<PurchaseLineItem> Cart { get; set; }
        public PaymentInfo Payment { get; set; }
    }
}