using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class SaleLineItem : Entity
    {
        public SaleLineItem(Product product, decimal quantity, decimal netPrice)
        {
            Type = LineItemType.Product;
            ItemId = product.Id;
            Name = product.Name;
            Quantity = quantity;
            UnitPurchasePrice = product.Price.Purchase;
            UnitPrice = netPrice / quantity;
        }

        public SaleLineItem(Product product, decimal quantity, decimal netPrice, decimal netPurchasePrice)
        {
            Type = LineItemType.Product;
            ItemId = product.Id;
            Name = product.Name;
            Quantity = quantity;
            UnitPurchasePrice = netPurchasePrice / quantity;
            UnitPrice = netPrice / quantity;
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

        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnitPurchasePrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal NetPrice => UnitPrice * Quantity;

        [DataType(DataType.Currency)]
        public decimal NetPurchasePrice => UnitPurchasePrice * Quantity;

        [DataType(DataType.Currency)]
        public decimal Profit => NetPrice - NetPurchasePrice;

        public int ItemId { get; set; }

        public LineItemType Type { get; set; }
    }
}
