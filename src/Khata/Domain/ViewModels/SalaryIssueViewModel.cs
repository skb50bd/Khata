using System.ComponentModel.DataAnnotations;

namespace ViewModels;

public class SalaryIssueViewModel
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }

    [Display(Name = "Amount Issued", ShortName = "Amount")]
    [DataType(DataType.Currency)]
    public decimal Amount { get; set; }

    [MaxLength(200)]
    public string Description { get; set; }
}