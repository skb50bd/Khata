namespace Domain;

public class Vouchar : Invoice
{
    public int? PurchaseId { get; set; }
    public virtual Purchase Purchase { get; set; }
    public int? SupplierPaymentId { get; set; }
    public virtual SupplierPayment SupplierPayment { get; set; }

    public int SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }
}