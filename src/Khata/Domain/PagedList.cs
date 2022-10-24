namespace Domain;

public interface IPagedList<T> : IPaginationProperties
{
    List<T> Items { get; }
}

public interface IPaginationProperties
{
    int PageIndex { get; }
    int PageSize { get; }
    int TotalCount { get; }
    int TotalPages { get; }
    bool HasPreviousPage { get; }
    bool HasNextPage { get; }
}

[Serializable]
public class PagedList<T> : IPagedList<T>
{
    public List<T> Items { get; } = new();

    public PagedList(IList<T> source, int pageIndex, int pageSize)
    {
        TotalCount = source.Count;
        TotalPages = TotalCount / pageSize;

        if (TotalCount % pageSize > 0)
        {
            TotalPages++;
        }

        PageSize = pageSize;
        PageIndex = pageIndex;
        Items.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
    }

    public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
    {
        TotalCount = totalCount;
        TotalPages = TotalCount / pageSize;

        if (TotalCount % pageSize > 0)
        {
            TotalPages++;
        }

        PageSize = pageSize;
        PageIndex = pageIndex;
        Items.AddRange(source);
    }

    /// <summary>
    /// Ctor
    /// </summary>
    /// <param name="source">source</param>
    /// <param name="paginationProperties">Pagination properties</param>
    public PagedList(IEnumerable<T> source, IPaginationProperties paginationProperties)
    {
        TotalCount = paginationProperties.TotalCount;
        TotalPages = paginationProperties.TotalPages;
        PageSize   = paginationProperties.PageSize;
        PageIndex  = paginationProperties.PageIndex;
        Items.AddRange(source);
    }

    public int PageIndex { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public int TotalPages { get; private set; }
    public bool HasPreviousPage => PageIndex > 0;
    public bool HasNextPage => PageIndex + 1 < TotalPages;
}