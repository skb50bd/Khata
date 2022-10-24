using System.ComponentModel.DataAnnotations;

namespace DTOs;

public class InvoiceCartItemDto
{
    public string Name { get; set; }
    public decimal Quantity { get; set; }

    [DataType(DataType.Currency)]
    public decimal UnitPrice { get; set; }

    [DataType(DataType.Currency)]
    public decimal NetPrice { get; set; }
}