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
using System.Threading;
using System.Threading.Tasks;
using GlasssixBulkAliasResponse = Glasssix.Contrib.Data.Elasticsearch.Response.Alias.BulkAliasResponse;
using GlasssixGetAliasResponse = Glasssix.Contrib.Data.Elasticsearch.Response.Alias.GetAliasResponse;

namespace Glasssix.Contrib.Data.Elasticsearch
{
    public interface IGlasssixElasticClient
    {
        #region index manage

        Task<Response.Index.CreateIndexResponse> CreateIndexAsync(
            string indexName,
            CreateIndexOptions? options = null,
            CancellationToken cancellationToken = default);
        Task<Response.Index.CreateIndexResponse> CreateIndexAsync(
            string indexName, Func<CreateIndexDescriptor, ICreateIndexRequest> selector = null,
            CancellationToken cancellationToken = default);

        Task<Response.Index.DeleteIndexResponse> DeleteIndexAsync(string indexName,
            CancellationToken cancellationToken = default);

        Task<Response.Index.DeleteIndexResponse> DeleteIndexByAliasAsync(
            string alias,
            CancellationToken cancellationToken = default);

        Task<Response.Index.DeleteIndexResponse> DeleteMultiIndexAsync(
            IEnumerable<string> indexNames,
            CancellationToken cancellationToken = default);

        Task<Response.Index.GetIndexResponse> GetAllIndexAsync(CancellationToken cancellationToken = default);

        Task<GetIndexByAliasResponse> GetIndexByAliasAsync(
            string alias,
            CancellationToken cancellationToken = default);

        Task<Response.ExistsResponse> IndexExistAsync(
                                    string indexName,
            CancellationToken cancellationToken = default);

        #endregion index manage

        #region alias manage

        Task<GlasssixBulkAliasResponse> BindAliasAsync(
            BindAliasIndexOptions options,
            CancellationToken cancellationToken = default);

        Task<GlasssixGetAliasResponse> GetAliasByIndexAsync(
            string indexName,
            CancellationToken cancellationToken = default);

        Task<GlasssixGetAliasResponse> GetAllAliasAsync(CancellationToken cancellationToken = default);

        Task<GlasssixBulkAliasResponse> UnBindAliasAsync(
            UnBindAliasIndexOptions options,
            CancellationToken cancellationToken = default);

        #endregion alias manage

        #region document manage

        Task<ClearDocumentResponse> ClearDocumentAsync(string indexNameOrAlias, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a new document
        /// only when the document does not exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        Task<Response.CreateResponse> CreateDocumentAsync<TDocument>(
            CreateDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// Add new documents in batches
        /// only when the documents do not exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        Task<CreateMultiResponse> CreateMultiDocumentAsync<TDocument>(
            CreateMultiDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default) where TDocument : class;

        Task<Response.DeleteResponse> DeleteDocumentAsync(
            DeleteDocumentRequest request,
            CancellationToken cancellationToken = default);

        Task<DeleteMultiResponse> DeleteMultiDocumentAsync(
            DeleteMultiDocumentRequest request,
            CancellationToken cancellationToken = default);

        Task<CountDocumentResponse> DocumentCountAsync(CountDocumentRequest request, CancellationToken cancellationToken = default);

        Task<Response.ExistsResponse> DocumentExistsAsync(
                                    ExistDocumentRequest request,
            CancellationToken cancellationToken = default);

        Task<Response.GetResponse<TDocument>> GetAsync<TDocument>(
                GetDocumentRequest request,
                CancellationToken cancellationToken = default) where TDocument : class;

        Task<Response.SearchResponse<TDocument>> GetListAsync<TDocument>(
                QueryOptions<TDocument> options,
                CancellationToken cancellationToken = default) where TDocument : class;

        Task<GetMultiResponse<TDocument>> GetMultiAsync<TDocument>(
                GetMultiDocumentRequest request,
                CancellationToken cancellationToken = default) where TDocument : class;

        Task<SearchPaginatedResponse<TDocument>> GetPaginatedListAsync<TDocument>(
                PaginatedOptions<TDocument> options,
                CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// Update or insert document
        /// Overwrite if it exists, add new if it does not exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        Task<SetResponse> SetDocumentAsync<TDocument>(
            SetDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default) where TDocument : class;

        /// <summary>
        /// Update the document
        /// only if the document exists
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        Task<UpdateResponse> UpdateDocumentAsync<TDocument>(
            UpdateDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default)
            where TDocument : class;

        /// <summary>
        /// Update documents in batches
        /// only when the documents exist
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        Task<UpdateMultiResponse> UpdateMultiDocumentAsync<TDocument>(
            UpdateMultiDocumentRequest<TDocument> request,
            CancellationToken cancellationToken = default)
            where TDocument : class;

        #endregion document manage
    }
}