using System.Collections.Generic;

namespace WebIdentityApi.Models
{
    public class PaginateDataView<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int totalCount { get; set; }
    }
}