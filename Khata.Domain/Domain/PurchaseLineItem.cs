﻿namespace Khata.Domain
{
    public class PurchaseLineItem : Entity
    {
        public PurchaseLineItem(Product product, decimal quantity, decimal unitPrice)
        {
            Name = product.Name;
            Quantity = quantity;
            UnitPurchasePrice = unitPrice;
            ProductId = product.Id;
        }
        private PurchaseLineItem()
        {

        }

        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPurchasePrice { get; set; }
        public decimal NetPurchasePrice => UnitPurchasePrice * Quantity;

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}