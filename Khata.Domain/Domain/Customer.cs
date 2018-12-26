using System.Collections.Generic;

namespace Khata.Domain
{
    public class Customer : TrackedEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Balance { get; set; }
        public decimal Debt => -1M * Balance;
        public Metadata Metadata { get; set; }

        public virtual ICollection<Sale> Purchases { get; set; }
        public virtual ICollection<DebtPayment> DebtPayments { get; set; }
    }
}
