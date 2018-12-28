using System;

namespace Khata.Services.PageFilterSort
{
    public class Sieve
    {
        public string Filter { get; set; }
        public string SearchArea { get; set; }
        public int ResultsCount { get; set; }
        public int SentCount { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)ResultsCount / PageSize);
        public bool HasPage(int pageNumber) => pageNumber <= TotalPages;
        public bool HasPrevPage => PageIndex > 1;
        public bool HasNextPage => HasPage(PageIndex + 1);
    }
}