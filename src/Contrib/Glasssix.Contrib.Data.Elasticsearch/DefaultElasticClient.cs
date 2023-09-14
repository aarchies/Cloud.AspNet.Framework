using Elasticsearch.Net;
using Glasssix.Contrib.Data.Elasticsearch.Options.Alias;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Count;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Create;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Delete;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Exist;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Get;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Query;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Set;
using Glasssix.Contrib.Data.Elasticsearch.Options.Document.Update;
using Glasssix.Contrib.Data.Elasticsearch.Options.Index;
using Glasssix.Contrib.Data.Elasticsearch.Response;
using Glasssix.Contrib.Data.Elasticsearch.Response.Document;
using Glasssix.Contrib.Data.Elasticsearch.Response.Index;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GlasssixBulkAliasResponse = Glasssix.Contrib.Data.Elasticsearch.Response.Alias.BulkAliasResponse;
using GlasssixGetAliasResponse = Glasssix.Contrib.Data.Elasticsearch.Response.Alias.GetAliasResponse;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    public class DefaultGlasssixElasticClient : IGlasssixElasticClient
    {
        private readonly IElasticClient _elasticClient;

        public DefaultGlasssixElasticClient(IElasticClient elasticClient)
            => _elasticClient = elasticClient;

        #region index manage

        public async Task<Response.Index.CreateIndexResponse> CreateIndexAsync(
            string indexName,
            CreateIndexOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            ICreateIndexRequest request = new CreateIndexRequest(indexName);
            if (options != null)
            {
                request.Settings = options.IndexSettings;
                request.Aliases = options.Aliases;
                request.Mappings = options.Mappings;
            }

            var result = await _elasticClient.Indices.CreateAsync(request, cancellationToken);
            return new Response.Index.CreateIndexResponse(result);
        }



        public async Task<Response.Index.CreateIndexResponse> CreateIndexAsync(
            string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> selector = null,
            CancellationToken cancellationToken = default)
        {

            var result = await _elasticClient.Indices.CreateAsync(indexName, selector, cancellationToken);
            return new Response.Index.CreateIndexResponse(result);
        }



        public async Task<Response.Index.DeleteIndexResponse> DeleteIndexAsync(
            string indexName,
            CancellationToken cancellationToken = default)
        {
            IDeleteIndexRequest request = new DeleteIndexRequest(indexName);
            return new Response.Index.DeleteIndexResponse(await _elasticClient.Indices.DeleteAsync(request, cancellationToken));
        }

        public async Task<Response.Index.DeleteIndexResponse> DeleteIndexByAliasAsync(
            string alias,
            CancellationToken cancellationToken = default)
        {
            var response = await GetIndexByAliasAsync(alias, cancellationToken);
            if (response.IsValid)
                return await DeleteMultiIndexAsync(response.IndexNames, cancellationToken);

            return new Response.Index.DeleteIndexResponse(response.Message);
        }

        public async Task<Response.Index.DeleteIndexResponse> DeleteMultiIndexAsync(
            IEnumerable<string> indexNames,
            CancellationToken cancellationToken = default)
        {
            BulkAliasDescriptor request = new BulkAliasDescriptor();
            foreach (var indexName in indexNames)
                request.RemoveIndex(opt => opt.Index(indexName));

            return new Response.Index.DeleteIndexResponse(await _elasticClient.Indices.BulkAliasAsync(request, cancellationToken));
        }

        public async Task<Response.Index.GetIndexResponse> GetAllIndexAsync(CancellationToken cancellationToken = default)
        {
            ICatIndicesRequest request = new CatIndicesRequest();
            var response = await _elasticClient.Cat.IndicesAsync(request, cancellationToken);
            return new Response.Index.GetIndexResponse(response);
        }

        public async Task<GetIndexByAliasResponse> GetIndexByAliasAsync(string alias, CancellationToken cancellationToken = default)
        {
            ICatIndicesRequest request = new CatIndicesRequest(alias);
            var response = await _elasticClient.Cat.IndicesAsync(request, cancellationToken);
            return new GetIndexByAliasResponse(response);
        }

        public async Task<Response.ExistsResponse> IndexExistAsync(
                                    string indexName,
            CancellationToken cancellationToken = default)
        {
            IIndexExistsRequest request = new IndexExistsRequest(indexName);
            return new Response.ExistsResponse(await _elasticClient.Indices.ExistsAsync(request, cancellationToken));
        }

        #endregion index manage

        #region alias manage

        public async Task<GlasssixBulkAliasResponse> BindAliasAsync(
            BindAliasIndexOptions options,
            CancellationToken cancellationToken = default)
        {
            BulkAliasDescriptor request = new BulkAliasDescriptor();
            foreach (var indexName in options.IndexNames)
                request.Add(opt => opt.Aliases(options.Alias).Index(indexName));

            var response = await _elasticClient.Indices.BulkAliasAsync(request, cancellationToken);
            return new GlasssixBulkAliasResponse(response);
        }

        public async Task<GlasssixGetAliasResponse> GetAliasByIndexAsync(
            string indexName,
            CancellationToken cancellationToken = default)
        {
            IGetAliasRequest request = new GetAliasRequest((Indices)indexName);
            var response = await _elasticClient.Indices.GetAliasAsync(request, cancellationToken);
            return new GlasssixGetAliasResponse(response);
        }

        public async Task<GlasssixGetAliasResponse> GetAllAliasAsync(CancellationToken cancellationToken = default)
        {
            Func<CatAliasesDescriptor, ICatAliasesRequest>? selector = null;
            var response = await _elasticClient.Cat.AliasesAsync(selector, cancellationToken);
            return new GlasssixGetAliasResponse(response);
        }

        public async Task<GlasssixBulkAliasResponse> UnBindAliasAsync(
            UnBindAliasIndexOptions options,
            CancellationToken cancellationToken = default)
        {
            BulkAliasDescriptor request = new BulkAliasDescriptor();
            foreach (var indexName in options.IndexNames)
                request.Remove(opt => opt.Aliases(options.Alias).Index(indexName));

            var response = await _elasticClient.Indices.BulkAliasAsync(request, cancellationToken);
            return new GlasssixBulkAliasResponse(response);
        }

        #endregion alias manage

        #region document manage

        public async Task<ClearDocumentResponse> ClearDocumentAsync(string indexNameOrAlias, CancellationToken cancellationToken = default)
        {
            var deleteByQueryRequest = new DeleteByQueryRequest(indexNameOrAlias)
            {
                Query = new QueryContainer(new MatchAllQuery())
            };
            return new ClearDocumentResponse(await _elasticClient.DeleteByQueryAsync(deleteByQueryRequest, cancellationToken));
        }

        /// <summary>
        /// Add a new document
        /// only when the document does not exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        public async Task<Response.CreateResponse> CreateDocumentAsync<TDocument>(
            CreateDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default) where TDocument : class
        {
            ICreateRequest<TDocument> createRequest = new CreateRequest<TDocument>(request.IndexName, new Id(request.Request.DocumentId));
            createRequest.Document = request.Request.Document;
            return new Response.CreateResponse(await _elasticClient.CreateAsync(createRequest, cancellationToken));
        }

        /// <summary>
        /// Add new documents in batches
        /// only when the documents do not exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        public async Task<CreateMultiResponse> CreateMultiDocumentAsync<TDocument>(
            CreateMultiDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default) where TDocument : class
        {
            BulkDescriptor descriptor = new BulkDescriptor(request.IndexName);
            foreach (var item in request.Items)
            {
                descriptor
                    .Create<TDocument>(opt => opt.Document(item.Document)
                        .Index(request.IndexName)
                        .Id(item.DocumentId));
            }

            var response = await _elasticClient.BulkAsync(descriptor, cancellationToken);
            return new CreateMultiResponse(response);
        }

        public async Task<Response.DeleteResponse> DeleteDocumentAsync(
            DeleteDocumentRequest request,
            CancellationToken cancellationToken = default)
        {
            IDeleteRequest deleteRequest = new DeleteRequest(request.IndexName, new Id(request.DocumentId));
            return new Response.DeleteResponse(await _elasticClient.DeleteAsync(deleteRequest, cancellationToken));
        }

        public async Task<DeleteMultiResponse> DeleteMultiDocumentAsync(
            DeleteMultiDocumentRequest request,
            CancellationToken cancellationToken = default)
        {
            var response = await Internal.DeleteMultiExtensions.DeleteManyAsync(_elasticClient, request.DocumentIds, request.IndexName, cancellationToken);
            return new DeleteMultiResponse(response);
        }

        public async Task<CountDocumentResponse> DocumentCountAsync(CountDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var countRequest = new CountRequest(request.IndexName)
            {
                ExpandWildcards = ExpandWildcards.All
            };
            return new CountDocumentResponse(await _elasticClient.CountAsync(countRequest, cancellationToken));
        }

        public async Task<Response.ExistsResponse> DocumentExistsAsync(
                                    ExistDocumentRequest request,
            CancellationToken cancellationToken = default)
        {
            var documentExistsRequest = new DocumentExistsRequest(request.IndexName, request.DocumentId);
            return new Response.ExistsResponse(await _elasticClient.DocumentExistsAsync(documentExistsRequest, cancellationToken));
        }

        public async Task<Response.GetResponse<TDocument>> GetAsync<TDocument>(
                GetDocumentRequest request,
                CancellationToken cancellationToken = default) where TDocument : class
        {
            IGetRequest getRequest = new GetRequest(request.IndexName, request.Id);


            return new Response.GetResponse<TDocument>(await _elasticClient.GetAsync<TDocument>(getRequest, cancellationToken));
        }

        public async Task<Response.SearchResponse<TDocument>> GetListAsync<TDocument>(
                QueryOptions<TDocument> options,
                CancellationToken cancellationToken = default) where TDocument : class
        {
            var response = await QueryString(
                options.IndexName,
                options.Skip,
                options.Take,
                options,
                cancellationToken);
            return new Response.SearchResponse<TDocument>(response);
        }

        public async Task<GetMultiResponse<TDocument>> GetMultiAsync<TDocument>(
                GetMultiDocumentRequest request,
                CancellationToken cancellationToken = default) where TDocument : class
        {
            var response = (await
                    _elasticClient.GetManyAsync<TDocument>(request.Ids, request.IndexName, cancellationToken)
                )?.ToList() ?? new List<IMultiGetHit<TDocument>>();

            if (response.Count == request.Ids.Count())
                return new GetMultiResponse<TDocument>(true, "success", response);

            return new GetMultiResponse<TDocument>(false, "Failed to get document");
        }

        public async Task<SearchPaginatedResponse<TDocument>> GetPaginatedListAsync<TDocument>(
                PaginatedOptions<TDocument> options,
                CancellationToken cancellationToken = default) where TDocument : class
        {
            var response = await QueryString(
                options.IndexName,
                (options.Page - 1) * options.PageSize,
                options.PageSize,
                options,
                cancellationToken);
            return new SearchPaginatedResponse<TDocument>(options.PageSize, response);
        }

        /// <summary>
        /// Update or insert document
        /// Overwrite if it exists, add new if it does not exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        public async Task<SetResponse> SetDocumentAsync<TDocument>(
            SetDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default) where TDocument : class
        {
            BulkDescriptor descriptor = new BulkDescriptor(request.IndexName);
            foreach (var item in request.Items)
            {
                descriptor
                    .Index<TDocument>(opt => opt.Document(item.Document)
                        .Index(request.IndexName)
                        .Id(item.DocumentId));
            }

            var response = await _elasticClient.BulkAsync(descriptor, cancellationToken);
            return new SetResponse(response);
        }

        /// <summary>
        /// Update the document
        /// only if the document exists
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public async Task<UpdateResponse> UpdateDocumentAsync<TDocument>(
            UpdateDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default)
            where TDocument : class
        {
            if (request.Request.Document != null)
            {
                var response = await _elasticClient.UpdateAsync<TDocument>(
                    request.Request.DocumentId,
                    opt => opt.Doc(request.Request.Document).Index(request.IndexName),
                    cancellationToken);
                return new UpdateResponse(response);
            }

            IUpdateRequest<TDocument, object> updateRequest =
                new UpdateRequest<TDocument, object>(request.IndexName, request.Request.DocumentId)
                {
                    Doc = request.Request.PartialDocument!
                };
            return new UpdateResponse(await _elasticClient.UpdateAsync(updateRequest, cancellationToken));
        }

        /// <summary>
        /// Update documents in batches
        /// only when the documents exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        public async Task<UpdateMultiResponse> UpdateMultiDocumentAsync<TDocument>(
            UpdateMultiDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default)
            where TDocument : class
        {
            BulkDescriptor descriptor = new BulkDescriptor(request.IndexName);
            foreach (var item in request.Items)
            {
                if (item.Document != null)
                {
                    descriptor
                        .Update<TDocument>(opt => opt.Doc(item.Document)
                            .Index(request.IndexName)
                            .Id(item.DocumentId));
                }
                else
                {
                    descriptor
                        .Update<TDocument, object>(opt => opt.Doc(item.PartialDocument!)
                            .Index(request.IndexName)
                            .Id(item.DocumentId));
                }
            }

            var response = await _elasticClient.BulkAsync(descriptor, cancellationToken);
            return new UpdateMultiResponse(response);
        }

        private static QueryStringQueryDescriptor<TDocument> GetQueryDescriptor<TDocument>(
            QueryStringQueryDescriptor<TDocument> queryDescriptor,
            QueryBaseOptions<TDocument> queryBaseOptions)
            where TDocument : class
        {
            queryDescriptor = queryDescriptor.Query(queryBaseOptions.Query);
            if (!string.IsNullOrEmpty(queryBaseOptions.Analyzer))
                queryDescriptor = queryDescriptor.Analyzer(queryBaseOptions.Analyzer);

            queryDescriptor = queryDescriptor.DefaultOperator(queryBaseOptions.Operator);
            if (!string.IsNullOrEmpty(queryBaseOptions.DefaultField))
                queryDescriptor.DefaultField(queryBaseOptions.DefaultField);

            if (queryBaseOptions.Fields.Any())
                queryDescriptor.Fields(queryBaseOptions.Fields.ToArray());

            queryBaseOptions.Action?.Invoke(queryDescriptor);

            return queryDescriptor;
        }

        private Task<ISearchResponse<TDocument>> QueryString<TDocument>(
                string? indexName,
            int skip,
            int take,
            QueryBaseOptions<TDocument> queryBaseOptions,
            CancellationToken cancellationToken = default)
            where TDocument : class
        {
            return _elasticClient.SearchAsync<TDocument>(s => s
                .Index(indexName)
                .From(skip)
                .Size(take)
                .Query(q => q
                    .QueryString(qs => GetQueryDescriptor(qs, queryBaseOptions))
                ), cancellationToken);
        }

        #endregion document manage
    }
}