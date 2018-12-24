using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        public ITrackingRepository<Product> Products { get; set; }


        protected KhataContext Context { get; }
        public UnitOfWork(KhataContext context, ITrackingRepository<Product> products)
        {
            Context = context;
            Products = products;
        }

        public void Complete()
        {
            Context.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}