using Domain;
using Microsoft.EntityFrameworkCore;

namespace Data.Persistence;

public static class QueryablesExtensions
{
    public static async Task<PagedList<T>> ToPagedListAsync<T>(
        this IQueryable<T> source, 
        int pageIndex = 0,
        int pageSize = 40
    )
    {
        var countTask = source.CountAsync();
        var itemsTask = source.Skip(pageIndex * pageSize).Take(pageSize).ToListAsync();
        await Task.WhenAll(countTask, itemsTask);
        
        var totalCount = await countTask;
        var items = await itemsTask;
        return new PagedList<T>(items, pageIndex, pageSize, totalCount);
    }
}