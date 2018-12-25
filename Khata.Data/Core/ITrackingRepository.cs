using System.Collections.Generic;
using System.Threading.Tasks;

using Khata.Domain;

namespace Khata.Data.Core
{
    public interface ITrackingRepository<T> : IRepository<T> where T : TrackedEntity
    {
        Task Remove(int id);
        Task<IList<T>> GetRemovedItems();
        Task<bool> IsRemoved(int id);

    }
}
