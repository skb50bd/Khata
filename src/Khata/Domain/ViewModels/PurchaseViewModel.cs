using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class PurchaseViewModel
{
    public int? Id { get; set; }
    public int SupplierId { get; set; }

    [Display(Name = "Add New Supplier")]
    public bool RegisterNewSupplier { get; set; }

    [Display(Name = "Seller")]
    public SupplierViewModel Supplier { get; set; }

    [Display(Name = "Cart")]
    public List<LineItemViewModel> Cart { get; set; }

    public PaymentInfoViewModel Payment { get; set; } = new PaymentInfoViewModel();

    [Display(Name = "Date")]
    public string PurchaseDate { get; set; }

    public string Description { get; set; }
}