namespace Khata.Domain
{
    public class LineItem : Entity
    {
        public LineItem(Product product, decimal quantity, decimal unitPrice)
        {
            _isProduct = true;
            ItemId = product.Id;
            Name = product.Name;
            Quantity = quantity;
            Price = unitPrice;
            NetPrice = unitPrice * quantity;
        }

        public LineItem(Service service, decimal price)
        {
            _isProduct = false;
            ItemId = service.Id;
            Name = service.Name;
            Quantity = 1;
            NetPrice = price;
            Price = price;
        }


        public string Name { get; }
        public decimal Quantity { get; }
        public decimal Price { get; }
        public decimal NetPrice { get; }

        public int ItemId { get; set; }

        private readonly bool _isProduct;
    }
}
