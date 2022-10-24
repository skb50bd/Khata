namespace Domain;

public class Supplier : Person
{
    public required string CompanyName { get; set; }
    public decimal Payable { get; set; }
    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    public virtual ICollection<SupplierPayment> Payments { get; set; } = new List<SupplierPayment>();
    public virtual ICollection<PurchaseReturn> PurchaseReturns { get; set; } = new List<PurchaseReturn>();
}