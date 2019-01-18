namespace Khata.Domain
{
    public interface ITransaction
    {
        decimal Amount { get; }
        string TableName { get; }
        int RowId { get; }
        string Description { get; }
    }

    public interface IWithdrawal : ITransaction { }

    public interface IDeposit : ITransaction { }

    public abstract class Transaction : Entity, ITransaction
    {
        public decimal Amount { get; set; }
        public string TableName { get; set; }
        public int RowId { get; set; }
        public string Description { get; set; }
    }

    public class Withdrawal : Transaction
    {
        private Withdrawal() { }

        public Withdrawal(IWithdrawal item)
        {
            Amount = item.Amount;
            TableName = item.TableName;
            RowId = item.RowId;
            Description = item.Description;
        }
    }

    public class Deposit : Transaction
    {
        private Deposit() { }

        public Deposit(IDeposit item)
        {
            Amount = item.Amount;
            TableName = item.TableName;
            RowId = item.RowId;
            Description = item.Description;
        }
    }
}
