using Glasssix.Contrib.Data.Elasticsearch.Internal.BulkOperation;
using Nest;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Glasssix.Contrib.Data.Elasticsearch.Internal
{
    internal static class DeleteMultiExtensions
    {
        public static Task<BulkResponse> DeleteManyAsync(
            this IElasticClient client,
            IEnumerable<string> documentIds,
            IndexName index,
            CancellationToken cancellationToken = default)
        {
            var bulkRequest = CreateDeleteBulkRequest(documentIds, index);
            return client.BulkAsync(bulkRequest, cancellationToken);
        }

        private static BulkRequest CreateDeleteBulkRequest(IEnumerable<string> documentIds, IndexName index)
        {
            //ArgumentNullException.ThrowIfNull(documentIds, nameof(documentIds));
            var bulkRequest = new BulkRequest(index);
            var deletes = documentIds
                .Select(id => new BulkDeleteOperation(new Id(id)))
                .Cast<IBulkOperation>()
                .ToList();

            bulkRequest.Operations = new BulkOperationsCollection<IBulkOperation>(deletes);
            return bulkRequest;
        }
    }
}