using System.Collections.Generic;

namespace Domain;

public class Supplier : Person
{
    public string CompanyName { get; set; }
    public decimal Payable { get; set; }

    public virtual ICollection<Purchase> Purchases { get; set; }
    public virtual ICollection<SupplierPayment> Payments { get; set; }
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; }
}