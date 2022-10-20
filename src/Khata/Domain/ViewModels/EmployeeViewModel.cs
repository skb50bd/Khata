using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class EmployeeViewModel : PersonViewModel
{
    [Display(Name = "NID Number")]
    public string NIdNumber { get; set; }
    [DataType(DataType.Currency)]
    public decimal Balance { get; set; }
    public string Designation { get; set; }
    public decimal Salary { get; set; }
}