using System.Collections.Generic;
using System.Threading.Tasks;

using Data.Core;

using Domain;

namespace Data.Persistence.Repositories;

/// <summary>
/// This Repository Removes Unit of Work from the Repository and forces actions to be 
/// atomic.  When there are not business reasons to do multiple actions in a single transaction
/// this allows the business layer to ignore the 
/// </summary>
/// <typeparam name="T"></typeparam>
public class AtomicRepository<T> : Repository<T>, IRepository<T> where T : Document
{
    public AtomicRepository(KhataContext context) : base(context) { }

    public override async void Add(T item)
    {
        base.Add(item);
        await Context.SaveChangesAsync();
    }

    public override async void AddAll(IEnumerable<T> items)
    {
        base.AddAll(items);
        await Context.SaveChangesAsync();
    }

    public override async Task Delete(int id)
    {
        await base.Delete(id);
        await Context.SaveChangesAsync();
    }
}