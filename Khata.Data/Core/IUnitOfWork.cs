using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data.Core
{
    public interface IUnitOfWork
    {
        ITrackingRepository<Product> Products { get; }
        ITrackingRepository<Service> Services { get; }
        ITrackingRepository<Customer> Customers { get; }

        void Complete();

        Task CompleteAsync();
    }
}