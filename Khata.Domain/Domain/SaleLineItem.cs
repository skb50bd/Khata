namespace Khata.Domain
{
    public class SaleLineItem : Entity
    {
        public SaleLineItem(Product product, decimal quantity, decimal unitPrice)
        {
            Type = LineItemType.Product;
            ItemId = product.Id;
            Name = product.Name;
            Quantity = quantity;
            UnitPurchasePrice = product.Price.Purchase;
            UnitPrice = unitPrice;
        }

        public SaleLineItem(Service service, decimal price)
        {
            Type = LineItemType.Service;
            ItemId = service.Id;
            Name = service.Name;
            Quantity = 1;
            UnitPrice = price;
            UnitPurchasePrice = 0;
        }

        private SaleLineItem() { }


        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPurchasePrice { get; set; }
        public decimal NetPrice => UnitPrice * Quantity;
        public decimal NetPurchasePrice => UnitPurchasePrice * Quantity;

        public int ItemId { get; set; }

        public LineItemType Type { get; set; }
    }
}
