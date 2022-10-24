using Data.Core;

using Domain;
using Throw;

namespace Data.Persistence.Repositories;

/// <summary>
/// This Repository Removes Unit of Work from the Repository and forces actions to be 
/// atomic.  When there are not business reasons to do multiple actions in a single transaction
/// this allows the business layer to ignore the 
/// </summary>
/// <typeparam name="T"></typeparam>
public class AtomicRepository<T> : Repository<T> where T : Document
{
    public AtomicRepository(
            KhataContext context, 
            IDateTimeProvider dateTime) 
        : base(context, dateTime) 
    { }

    public override async Task Add(T item, bool saveChanges = true)
    {
        saveChanges.Throw().IfFalse();
        await base.Add(item, saveChanges);
    }

    public override async Task AddAll(IEnumerable<T> items, bool saveChanges = true)
    {
        saveChanges.Throw().IfFalse();
        await base.AddAll(items, saveChanges);
    }

    public override async Task Delete(int id, bool saveChanges = true)
    {
        saveChanges.Throw().IfFalse();
        await base.Delete(id, saveChanges);
    }
}