using System.Linq;
using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

using Microsoft.EntityFrameworkCore;

namespace Khata.Data.Persistence
{
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
}