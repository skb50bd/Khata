using System.ComponentModel.DataAnnotations;

namespace DTOs;

public class CustomerDto : PersonDto
{

    [Display(Name = "Company")]
    public string CompanyName { get; set; }

    [Display(Name = "Due")]
    [DataType(DataType.Currency)]
    public decimal Debt { get; set; }
}