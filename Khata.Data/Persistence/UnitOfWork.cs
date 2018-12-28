using System.Threading.Tasks;

using Khata.Data.Core;
using Khata.Domain;

namespace Khata.Data.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        protected readonly KhataContext Context;

        public ITrackingRepository<Product> Products { get; }
        public ITrackingRepository<Service> Services { get; }
        public ITrackingRepository<Customer> Customers { get; }
        public ITrackingRepository<DebtPayment> DebtPayments { get; }


        public UnitOfWork(KhataContext context,
            ITrackingRepository<Product> products,
            ITrackingRepository<Service> services,
            ITrackingRepository<Customer> customers,
            ITrackingRepository<DebtPayment> debtPayments)
        {
            Context = context;
            Products = products;
            Services = services;
            Customers = customers;
            DebtPayments = debtPayments;
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