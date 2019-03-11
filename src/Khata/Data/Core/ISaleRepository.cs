using System.Collections.Generic;
using System.Threading.Tasks;

using Domain;

namespace Data.Core
{
    public interface ISaleRepository : ITrackingRepository<Sale>
    {
        void Save(SavedSale model);
        Task DeleteAllSaved();
        Task DeleteSaved(int id);
        Task<SavedSale> GetSaved(int id);
        Task<IEnumerable<SavedSale>> GetSaved();
    }
}