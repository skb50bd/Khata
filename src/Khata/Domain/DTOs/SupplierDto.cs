using System.ComponentModel.DataAnnotations;

namespace DTOs;

public class SupplierDto : PersonDto
{

    [Display(Name = "Company")]
    public string CompanyName { get; set; }

    [DataType(DataType.Currency)]
    public decimal Payable { get; set; }
}