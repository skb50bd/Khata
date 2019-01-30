using System.ComponentModel.DataAnnotations;

namespace Khata.DTOs
{
    public class InvoiceLineItemDto
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }

        [DataType(DataType.Currency)]
        public decimal UnitPrice { get; set; }

        [DataType(DataType.Currency)]
        public decimal NetPrice { get; set; }
    }
}