using Glasssix.Contrib.Data.Elasticsearch.Response;
using Nest;
using System;
using System.Linq;

namespace Glasssix.Contrib.Data.Elasticsearch.Response.Index
{
    public class GetIndexByAliasResponse : ResponseBase
    {
        public GetIndexByAliasResponse(CatResponse<CatIndicesRecord> catResponse) : base(catResponse)
        {
            IndexNames = catResponse.IsValid ? catResponse.Records.Select(r => r.Index).ToArray() : Array.Empty<string>();
        }

        public string[] IndexNames { get; }
    }
}