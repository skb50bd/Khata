using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

namespace Khata.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly KhataContext _context;

        public ITrackingRepository<Product> Products { get; }


        public UnitOfWork(KhataContext context, ITrackingRepository<Product> products)
        {
            _context = context;
            Products = products;
        }

        public void Complete()
        {
            _context.SaveChanges();
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}