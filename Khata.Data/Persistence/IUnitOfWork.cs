using System.Threading.Tasks;
using Khata.Domain;

namespace Khata.Data
{
    public interface IUnitOfWork
    {
        ITrackingRepository<Product> Products { get; set; }

        void Complete();
        Task CompleteAsync();
    }
}