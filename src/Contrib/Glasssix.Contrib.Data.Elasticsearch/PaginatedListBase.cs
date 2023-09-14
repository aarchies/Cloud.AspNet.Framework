using System.Collections.Generic;

namespace Glasssix.Contrib.Data.Elasticsearch
{

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class BasePaginatedList<TEntity> : PaginatedListBase<TEntity>
        where TEntity : class
    {
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class PaginatedListBase<TEntity>
        where TEntity : class
    {
        public List<TEntity> Result { get; set; } = default!;
        public long Total { get; set; }

        public int TotalPages { get; set; }
    }
}