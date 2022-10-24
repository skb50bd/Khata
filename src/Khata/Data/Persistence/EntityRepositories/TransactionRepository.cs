using Data.Core;

using Domain;
using Microsoft.EntityFrameworkCore;
using Throw;

namespace Data.Persistence.Repositories;

public class DepositRepository : Repository<Deposit>, IRepository<Deposit>
{
    public DepositRepository(
            KhataContext context,
            IDateTimeProvider dateTimeProvider) 
        : base(context, dateTimeProvider) 
    { }
    
    public override async Task Add(Deposit item, bool saveChanges = true)
    {
        Context.Set<Deposit>().Add(item);
        var cashRegister = await Context.Set<CashRegister>().FirstOrDefaultAsync();
        cashRegister.ThrowIfNull();
        cashRegister.Balance += item.Amount;

        if (saveChanges)
        {
            await Context.SaveChangesAsync();
        }
    }
}

public class WithdrawalRepository : Repository<Withdrawal>, IRepository<Withdrawal>
{
    public WithdrawalRepository(
            KhataContext context,
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }
    
    public override async Task Add(Withdrawal item, bool saveChanges = true)
    {
        Context.Set<Withdrawal>().Add(item);
        
        var cashRegister = await Context.Set<CashRegister>().FirstOrDefaultAsync();
        cashRegister.ThrowIfNull();
        cashRegister.Balance -= item.Amount;

        if (saveChanges)
        {
            await Context.SaveChangesAsync();
        }
    }
}