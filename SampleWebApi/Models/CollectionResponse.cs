using System;
using System.Collections.Generic;
using System.Linq;

namespace SampleWebApi.Models
{
    public class CollectionResponse<T>
    {
        public CollectionResponse() : this(1, 0, Array.Empty<T>()) {  }

        public CollectionResponse(int page, int total, IEnumerable<T> items)
        {
            Page  = page;
            Total = total;
            Items = items.ToArray();
        }

        public int Page { get; set; }
        public long Total { get; set; }
        public ICollection<T> Items { get; set; }
    }
}