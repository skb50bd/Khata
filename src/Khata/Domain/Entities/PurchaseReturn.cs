namespace Domain;

public class PurchaseReturn : TrackedDocument, IDeposit
{
    public int SupplierId { get; set; }

    public virtual Supplier? Supplier { get; set; }

    public int PurchaseId { get; set; }

    public virtual Purchase? Purchase { get; set; }

    public virtual ICollection<PurchaseCartItem> Cart { get; set; } = new List<PurchaseCartItem>();

    public decimal CashBack { get; set; }

    public decimal DebtRollback { get; set; }

    public decimal TotalBackPaid => CashBack + DebtRollback;

    public string? Description { get; set; }

    public decimal TotalPrice => Cart?.Sum(li => li.NetPurchasePrice) ?? 0;

    public decimal Amount => CashBack;

    public string TableName => nameof(PurchaseReturn);

    public int? RowId => Id;
}