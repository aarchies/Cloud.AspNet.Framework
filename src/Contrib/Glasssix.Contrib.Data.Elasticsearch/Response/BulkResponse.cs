using System.Collections.Generic;
using System.Linq;

namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class BulkResponse : ResponseBase
    {
        public BulkResponse(Nest.BulkResponse bulkResponse) : base(bulkResponse)
        {
            Items = bulkResponse.Items.Select(item => new BulkResponseItems(item.Id, item.IsValid, item.Error?.ToString() ?? string.Empty)).ToList();
        }

        public List<BulkResponseItems> Items { get; set; }
    }
}