using System.Collections.Generic;

namespace Khata.Domain
{
    public class Customer : Person
    {
        public decimal Debt { get; set; }

        public virtual ICollection<Sale> Purchases { get; set; }
        public virtual ICollection<DebtPayment> DebtPayments { get; set; }
    }
}
