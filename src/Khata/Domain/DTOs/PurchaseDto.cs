using System.ComponentModel.DataAnnotations;

using Domain;

namespace DTOs;

public class PurchaseDto
{
    public int Id { get; set; }
    public bool IsRemoved { get; set; }

    [Display(Name = "Purchase Date")]
    [DataType(DataType.Date)]
    public DateTimeOffset PurchaseDate { get; set; }

    public int VoucharId { get; set; }
    public virtual Vouchar Vouchar { get; set; }

    public int SupplierId { get; set; }
    public SupplierDto Supplier { get; set; }

    public ICollection<PurchaseCartItem> Cart { get; set; }

    [Display(Name = "Subtotal")]
    [DataType(DataType.Currency)]
    public decimal PaymentSubTotal { get; set; }

    [Display(Name = "Total")]
    [DataType(DataType.Currency)]
    public decimal PaymentTotal { get; set; }


    [Display(Name = "Paid")]
    [DataType(DataType.Currency)]
    public decimal PaymentPaid { get; set; }

    [Display(Name = "Due")]
    [DataType(DataType.Currency)]
    public decimal PaymentDue { get; set; }

    public string Description { get; set; }

    public Metadata Metadata { get; set; }
}