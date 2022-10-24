namespace Domain;

public class Employee : Person
{
    public decimal Balance { get; set; }
    public required string Designation { get; set; }
    public decimal Salary { get; set; }
    public string? NIdNumber { get; set; }
    public virtual ICollection<SalaryIssue> SalaryIssues { get; set; } = new List<SalaryIssue>();
    public virtual ICollection<SalaryPayment> SalaryPayments { get; set; } = new List<SalaryPayment>();
}