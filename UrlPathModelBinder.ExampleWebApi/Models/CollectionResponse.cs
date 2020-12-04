using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleWebApi.Models
{
    public class CollectionResponse<T>
    {
        public CollectionResponse()
        {
            Items = Array.Empty<T>();
            Page = 1;
            Total = 0;
        }

        public CollectionResponse(CollectionRequest request, IQueryable<T> q)
        {
            Page = request.Page;
            Total = q.Count();
            Items = q.Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArray();
        }

        public int Page { get; set; }
        public long Total { get; set; }
        public ICollection<T> Items { get; set; }
    }
}