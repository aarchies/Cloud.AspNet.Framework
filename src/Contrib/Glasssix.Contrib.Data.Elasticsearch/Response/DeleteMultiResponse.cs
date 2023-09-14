using System.Collections.Generic;
using System.Linq;

namespace Glasssix.Contrib.Data.Elasticsearch.Response
{
    public class DeleteMultiResponse : ResponseBase
    {
        public DeleteMultiResponse(Nest.BulkResponse bulkResponse) : base(bulkResponse)
        {
            Data = bulkResponse.Items.Select(item =>
                new DeleteRangeResponseItems(item.Id,
                    item.IsValid && item.Status == 200,
                    !string.IsNullOrEmpty(item.Result) ? item.Result : item.Error?.ToString() ?? string.Empty)).ToList();
        }

        public List<DeleteRangeResponseItems> Data { get; set; }
    }
}