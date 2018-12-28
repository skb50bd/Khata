namespace Khata.Domain
{
    public class LineItem : Entity
    {
        public LineItem(Product product, decimal quantity, decimal unitPrice)
        {
            Type = LineItemType.Product;
            ItemId = product.Id;
            Name = product.Name;
            Quantity = quantity;
            Price = unitPrice;
            NetPrice = unitPrice * quantity;
        }

        public LineItem(Service service, decimal price)
        {
            Type = LineItemType.Service;
            ItemId = service.Id;
            Name = service.Name;
            Quantity = 1;
            NetPrice = price;
            Price = price;
        }

        private LineItem() { }


        public string Name { get; private set; }
        public decimal Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal NetPrice { get; private set; }

        public int ItemId { get; private set; }

        public LineItemType Type { get; private set; }
    }
}
