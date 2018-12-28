namespace Khata.DTOs
{
    public class LineItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public decimal NetPrice { get; set; }
    }
}