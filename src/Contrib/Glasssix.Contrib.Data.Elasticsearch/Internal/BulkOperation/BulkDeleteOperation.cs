using Nest;
using System;

namespace Glasssix.Contrib.Data.Elasticsearch.Internal.BulkOperation
{
    public class BulkDeleteOperation : BulkOperationBase
    {
        public BulkDeleteOperation(Id id) => Id = id;

        protected override Type ClrType { get; } = default!;

        protected override string Operation => "delete";

        protected override object GetBody() => null!;
    }
}