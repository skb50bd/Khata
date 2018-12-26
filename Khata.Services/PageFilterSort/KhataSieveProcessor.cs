using Khata.Domain;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;

namespace Khata.Services.PageFilterSort
{
    public class KhataSieveProcessor : SieveProcessor
    {
        public KhataSieveProcessor(
            IOptions<SieveOptions> options,
            ISieveCustomSortMethods customSortMethods,
            ISieveCustomFilterMethods customFilterMethods)
        : base(options, customSortMethods, customFilterMethods)
        { }

        protected override SievePropertyMapper MapProperties(SievePropertyMapper mapper)
        {
            #region Reference Code
            //mapper.Property<Post>(p => p.Title)
            //    .CanFilter()
            //    .HasName("a_different_query_name_here");

            //mapper.Property<Post>(p => p.CommentCount)
            //    .CanSort();

            //mapper.Property<Post>(p => p.DateCreated)
            //    .CanSort()
            //    .CanFilter()
            //    .HasName("created_on");
            #endregion

            mapper.Property<Product>(p => p.Name).CanFilter().CanSort();

            return mapper;
        }
    }
}