using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data.Core
{
    public interface IUnitOfWork
    {
        ITrackingRepository<Product> Products { get; }

        void Complete();

        Task CompleteAsync();
    }
}