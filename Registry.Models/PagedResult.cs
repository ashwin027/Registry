using System;
using System.Collections.Generic;
using System.Text;

namespace Registry.Models
{
    public class PagedResult<T>
    {
        public PagedResult()
        {
            Data = new List<T>();
        }
        public List<T> Data { get; set; }
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
