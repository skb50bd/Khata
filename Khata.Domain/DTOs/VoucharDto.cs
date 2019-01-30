namespace Khata.DTOs
{
    public class VoucharDto : InvoiceDto
    {
        public int? PurchaseId { get; set; }
        public virtual PurchaseDto Purchase { get; set; }
        public int? SupplierPaymentId { get; set; }
        public virtual SupplierPaymentDto SupplierPayment { get; set; }

        public int SupplierId { get; set; }
        public virtual SupplierDto Supplier { get; set; }
    }
}