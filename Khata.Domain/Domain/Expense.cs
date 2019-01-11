namespace Khata.Domain
{
    public class Expense : TrackedEntity
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
    }
}