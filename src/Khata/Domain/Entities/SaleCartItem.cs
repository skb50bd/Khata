using System.ComponentModel.DataAnnotations;

namespace Domain;

public class SaleCartItem : Entity
{
    public SaleCartItem(Product product, decimal quantity, decimal netPrice)
    {
        ProductId = product.Id;
        Name = product.Name;
        Quantity = quantity;
        UnitPurchasePrice = product.Price.Purchase;
        UnitPrice = netPrice / quantity;
    }

    public SaleCartItem(Product product, decimal quantity, decimal netPrice, decimal netPurchasePrice)
    {
        ProductId = product.Id;
        Name = product.Name;
        Quantity = quantity;
        UnitPurchasePrice = netPurchasePrice / quantity;
        UnitPrice = netPrice / quantity;
    }

    public SaleCartItem(Service service, decimal price)
    {
        ServiceId = service.Id;
        Name = service.Name;
        Quantity = 1;
        UnitPrice = price;
        UnitPurchasePrice = 0;
    }

    private SaleCartItem() { }
    
    public required string Name { get; set; }

    public required decimal Quantity { get; set; }

    [DataType(DataType.Currency)]
    public required decimal UnitPrice { get; set; }

    [DataType(DataType.Currency)]
    public decimal UnitPurchasePrice { get; set; }

    [DataType(DataType.Currency)]
    public decimal NetPrice => UnitPrice * Quantity;

    [DataType(DataType.Currency)]
    public decimal NetPurchasePrice => UnitPurchasePrice * Quantity;

    [DataType(DataType.Currency)]
    public decimal Profit => NetPrice - NetPurchasePrice;

    public int? ProductId { get; set; }
    public virtual Product? Product { get; set; }
    
    public int? ServiceId { get; set; }
    public virtual Service? Service { get; set; }

    public int? SaleId { get; set; }
    public virtual Sale? Sale {get; set; }
    
    public int? SavedSaleId { get; set; }
    public virtual SavedSale? SavedSale { get; set; }
    
    public int? RefundId { get; set; }
    public virtual Refund? Refund { get; set; }
    
    public LineItemType Type =>
        ProductId is not null
            ? LineItemType.Product
            : ServiceId is not null
                ? LineItemType.Service
                : throw new NotImplementedException();
}