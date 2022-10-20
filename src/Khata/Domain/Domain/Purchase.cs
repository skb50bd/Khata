using System;
using System.Collections.Generic;

namespace Domain;

public class Purchase : TrackedDocument, IWithdrawal
{
    public int SupplierId { get; set; }
    public virtual Supplier Supplier { get; set; }

    public int VoucharId { get; set; }
    public virtual Vouchar Vouchar { get; set; }

    public DateTimeOffset PurchaseDate { get; set; }
    public virtual ICollection<PurchaseLineItem> Cart { get; set; }
    public PaymentInfo Payment { get; set; }
    public string Description { get; set; }

    public decimal Amount => Payment.Paid;
    public string TableName => nameof(Purchase);
    public int? RowId => Id;
}