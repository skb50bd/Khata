using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class SupplierViewModel : PersonViewModel
{
    [Display(Name = "Company")]
    [MaxLength(200)]
    public string CompanyName { get; set; }

    [DataType(DataType.Currency)]
    public decimal Payable { get; set; }
}