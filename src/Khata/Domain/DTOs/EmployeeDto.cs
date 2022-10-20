using System.ComponentModel.DataAnnotations;

namespace DTOs;

public class EmployeeDto : PersonDto
{
    [DataType(DataType.Currency)]
    public decimal Balance { get; set; }

    public string Designation { get; set; }

    [DataType(DataType.Currency)]
    public decimal Salary { get; set; }

    [Display(Name = "NID Number")]
    public string NIdNumber { get; set; }
}