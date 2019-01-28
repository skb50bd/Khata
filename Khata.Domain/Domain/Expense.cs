namespace Khata.Domain
{
    public class Expense : TrackedEntity, IWithdrawal
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public string TableName => nameof(Expense);
        public int? RowId => Id;
    }
}