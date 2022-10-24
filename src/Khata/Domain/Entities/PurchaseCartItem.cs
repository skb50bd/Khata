using LanguageExt;

namespace Domain;

public class PurchaseCartItem : Entity
{
    public PurchaseCartItem(Product product, decimal quantity, decimal netPrice)
    {
        Name = product.Name;
        Quantity = quantity;
        UnitPurchasePrice = netPrice / Quantity;
        ProductId = product.Id;
    }
    
    private PurchaseCartItem()
    { }

    public int? PurchaseId { get; set; }
    public virtual Purchase? Purchase { get; set; }
    public int? PurchaseReturnId { get; set; }
    public virtual PurchaseReturn? PurchaseReturn { get; set; }
    
    public string Name { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPurchasePrice { get; set; }
    public decimal NetPurchasePrice => UnitPurchasePrice * Quantity;

    public int ProductId { get; set; }
    public virtual Product? Product { get; set; }
}