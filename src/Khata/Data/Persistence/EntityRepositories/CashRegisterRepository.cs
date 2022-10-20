using System.Linq;
using System.Threading.Tasks;

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
        if (!Context.CashRegister.AsNoTracking().Any())
        {
            var cr = new CashRegister
            {
                Metadata = Metadata.CreatedNew("system")
            };
            Context.CashRegister.Add(cr);
            Context.SaveChanges();
        }
    }

    public virtual async Task<CashRegister> Get()
        => await Context.CashRegister.FirstOrDefaultAsync();
}