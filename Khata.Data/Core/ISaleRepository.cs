using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data.Core
{
    public interface ISaleRepository : ITrackingRepository<Sale>
    {
        Task DeleteAllSavedSales();
        Task DeleteSavedSale(int id);
        Task<Sale> GetSavedSale(int id);
        Task<IEnumerable<Sale>> GetSavedSales();
    }
}