using Domain;

namespace Data.Core;

public interface ISaleRepository : ITrackingRepository<Sale>
{
    Task Save(SavedSale model);
    Task DeleteAllSaved();
    Task DeleteSaved(int id);
    Task<SavedSale?> GetSavedSale(int id);
    Task<IEnumerable<SavedSale>> GetSavedSales();
}