namespace Domain;

public class SalaryIssue : TrackedDocument
{
    public int EmployeeId { get; set; }
    public virtual Employee? Employee { get; set; }
    public decimal Amount { get; set; }
    public decimal BalanceBefore { get; set; }
    public decimal BalanceAfter => BalanceBefore + Amount;
    public string? Description { get; set; }
}