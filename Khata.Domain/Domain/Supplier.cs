using System.Collections.Generic;

namespace Khata.Domain
{
    public class Supplier : Person
    {
        public string CompanyName { get; set; }
        public decimal Payable { get; set; }

        public virtual ICollection<Purchase> Purchases { get; set; }
        public virtual ICollection<SupplierPayment> Payments { get; set; }
    }
}
