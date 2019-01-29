using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedLibrary
{
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int ResultCount { get; set; }

        public PaginationModel EmitPagination =>
            new PaginationModel
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                ResultCount = ResultCount,
                CurrentCount = Count
            };
        public IPagedList<TDestination> CastList<TDestination>(Func<T, TDestination> convert)
        {
            var res = new PagedList<TDestination>
            {
                PageIndex = PageIndex,
                PageSize = PageSize,
                ResultCount = ResultCount
            };

            ForEach(i => res.Add(convert(i)));
            return res;
        }
    }

    public class PaginationModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int ResultCount { get; set; }
        public int CurrentCount { get; set; }
        public int PageCount => (int)Math.Ceiling((decimal)ResultCount / PageSize);
        public bool HasNextPage => PageCount > PageIndex;
        public int NextPage => PageIndex + 1;
        public bool HasPrevPage => PageIndex > 1;
        public bool HasPage(int index) => 1 <= index && index <= PageCount;
        public int PrevPage => PageIndex == 1 ? 1 : PageIndex - 1;
    }

    public static class PagedListExtensions
    {
        public static IPagedList<T> GetPagedList<T>(this IEnumerable<T> source,
            int index, int size)
        {
            var res = new PagedList<T>
            {
                PageIndex = index,
                PageSize = size,
                ResultCount = source.Count()
            };
            res.AddRange(source.Skip((index - 1) * size).Take(size));

            return res;
        }


    }
}
