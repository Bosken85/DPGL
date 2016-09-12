using System;
using System.Collections.Generic;
using System.Linq;
using Digipolis.Web.Guidelines.Paging;

namespace Digipolis.Web.Guidelines.Models
{
    public class PagedResult<T> where T : class, new()
    {
        public IDictionary<string, ILink> Links { get; set; }

        public IEnumerable<T> Data { get; set; }

        public Page Page { get; set; }

        public PagedResult(int page, int pageSize, int totalElements, IEnumerable<T> data)
        {
            Data = data ?? new List<T>();
            Page = new Page
            {
                Number = page,
                Size = data.Count(),
                TotalElements = totalElements,
                TotalPages = (int)Math.Ceiling((double)totalElements / (double)pageSize)
            };
            Links = new Dictionary<string, ILink>();
        }
    }
}
