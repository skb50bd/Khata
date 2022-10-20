namespace Business.PageFilterSort;

public class PageFilter
{
    public PageFilter(string filter, int index, int size)
    {
        Filter    = filter?.ToLowerInvariant();
        PageIndex = index;
        PageSize  = size;
    }
    public string Filter { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}