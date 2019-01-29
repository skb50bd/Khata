using System;
using System.Collections.Generic;

namespace SharedLibrary
{
    public interface IPagedList<T> : IList<T>
    {
        int PageIndex { get; set; }
        int PageSize { get; set; }
        int ResultCount { get; set; }

        PaginationModel EmitPagination { get; }

        IPagedList<TDestination> CastList<TDestination>(
            Func<T, TDestination> convert);
    }
}