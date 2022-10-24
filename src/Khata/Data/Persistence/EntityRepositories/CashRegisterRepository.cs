using Data.Core;

using Domain;

using Microsoft.EntityFrameworkCore;

namespace Data.Persistence.Repositories;

public class CashRegisterRepository : ICashRegisterRepository
{
    protected readonly KhataContext Context;
    
    public CashRegisterRepository(KhataContext context)
    {
        Context = context;
        if (Context.Set<CashRegister>().AsNoTracking().Any())
        {
            return;
        }

        var cr = new CashRegister
        {
            Metadata = Metadata.CreatedNew("system")
        };
        
        Context.Set<CashRegister>().Add(cr);
        Context.SaveChanges();
    }

    public virtual async Task<CashRegister?> Get() => 
        await Context.Set<CashRegister>().FirstOrDefaultAsync();
}