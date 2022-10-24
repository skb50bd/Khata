namespace Domain;

public class SupplierPayment : TrackedDocument, IWithdrawal
{
    public DateTimeOffset PaymentDate { get; set; }

    public int SupplierId { get; set; }
    public virtual Supplier? Supplier { get; set; }
    public int VoucharId { get; set; }
    public virtual Vouchar? Vouchar { get; set; }
    public decimal PayableBefore { get; set; }
    public decimal Amount { get; set; }
    public decimal PayableAfter => PayableBefore - Amount;
    public string? Description { get; set; }

    public string TableName => nameof(SupplierPayment);
    public int? RowId => Id;
}