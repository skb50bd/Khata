using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

namespace Khata.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly KhataContext _context;

        public ITrackingRepository<Product> Products { get; }
        public ITrackingRepository<Service> Services { get; }


        public UnitOfWork(KhataContext context,
            ITrackingRepository<Product> products,
            ITrackingRepository<Service> services)
        {
            _context = context;
            Products = products;
            Services = services;
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