namespace Khata.Domain
{
    public class SupplierPayment : TrackedEntity
    {
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public decimal PayableBefore { get; set; }
        public decimal Amount { get; set; }
        public decimal PayableAfter => PayableBefore - Amount;
        public string Description { get; set; }
    }
}