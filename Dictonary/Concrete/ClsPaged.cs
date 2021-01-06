using NewsPortal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NewsPortal.Concrete
{
    public class ClsPaged<T>
    {
        public PagedCollection<T> Get(int? page, int? pageSize,List<T> Item)
        {
            var currPage = page.GetValueOrDefault(0);
            var currPageSize = pageSize.GetValueOrDefault(10);
            var paged = Item.Skip(currPage * currPageSize)
                                .Take(currPageSize)
                                .ToArray();

            var totalCount = Item.Count();

            return new PagedCollection<T>()
            {
                Page = currPage,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((decimal)totalCount / currPageSize),
                Items = paged
            };
        }
    }
}