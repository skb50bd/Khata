namespace Domain;

public class InvoiceCartItem : Entity
{
    public required string Name { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal NetPrice { get; set; }
    public int? InvoiceId { get; set; }
    public virtual CustomerInvoice? Invoice { get; set; }
    
    public int? VoucharId { get; set; }
    public virtual Vouchar? Vouchar { get; set; }
}