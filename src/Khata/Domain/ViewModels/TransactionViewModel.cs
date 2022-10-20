namespace ViewModels;

public interface ITransactionViewModel
{
    decimal Amount { get; }
    string TableName { get; }
    int? RowId { get; }
    string Description { get; }
}

public interface IWithdrawalViewModel : ITransactionViewModel { }

public interface IDepositViewModel : ITransactionViewModel { }

public abstract class TransactionViewModel : ITransactionViewModel
{
    public decimal Amount { get; set; }
    public string TableName { get; set; }
    public int? RowId { get; set; }
    public string Description { get; set; }
}

public class WithdrawalViewModel : TransactionViewModel
{
    public WithdrawalViewModel() { }

    public WithdrawalViewModel(IWithdrawalViewModel item)
    {
        Amount = item.Amount;
        TableName = item.TableName;
        RowId = item.RowId;
        Description = item.Description;
    }
}

public class DepositViewModel : TransactionViewModel
{
    public DepositViewModel() { }

    public DepositViewModel(IDepositViewModel item)
    {
        Amount = item.Amount;
        TableName = item.TableName;
        RowId = item.RowId;
        Description = item.Description;
    }
}