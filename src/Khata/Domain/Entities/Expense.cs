namespace Domain;

public class Expense : TrackedDocument, IWithdrawal
{
    public required string Name { get; set; }
    public required decimal Amount { get; set; }
    public string? Description { get; set; }

    public string TableName => nameof(Expense);
    public int? RowId => Id;
}