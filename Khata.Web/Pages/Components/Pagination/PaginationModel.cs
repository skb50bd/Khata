namespace WebUI.Pages.Components.Pagination
{
    public class PaginationModel
    {
        public string CurrentFilter { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int PageIndex { get; set; }
        public bool HasPage(int pageNumber) => pageNumber <= TotalPages;
        public bool HasPrevPage => PageIndex > 1;
        public bool HasNextPage => HasPage(PageIndex + 1);
    }
}
