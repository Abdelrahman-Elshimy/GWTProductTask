using System.Collections.Generic;

namespace Infrastracture.Core
{
    public class PaginatedResult<T>
    {
        public long TotalCount { get; set; }
        public IEnumerable<T> Page { get; set; }
    }
}
