namespace Domain
{
    public class InvoiceLineItem : Entity
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal NetPrice { get; set; }
    }
}