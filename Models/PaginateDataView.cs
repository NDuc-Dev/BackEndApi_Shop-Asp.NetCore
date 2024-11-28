using System.Collections.Generic;

namespace WebIdentityApi.Models
{
    public class PaginateDataView<T>
    {
        public IEnumerable<T> ListData { get; set; }
        public int totalCount { get; set; }
    }
}