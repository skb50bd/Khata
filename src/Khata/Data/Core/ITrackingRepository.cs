using Domain;

namespace Data.Core;

public interface ITrackingRepository<T> : IRepository<T> where T : TrackedDocument
{
    Task Remove(int id);
    Task<IList<T>> GetRemovedItems();
    Task<bool> IsRemoved(int id);
}