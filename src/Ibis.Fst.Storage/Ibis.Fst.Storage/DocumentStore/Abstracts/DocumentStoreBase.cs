using Ibis.Fst.Storage.Supports;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using Microsoft.Azure.Documents.Linq;
using System.Threading.Tasks;

namespace Ibis.Fst.Storage.DocumentStore
{
    public abstract class DocumentStoreBase
    {
        private readonly string _collectionId;
        private DocumentClient _documentClient;
        private string _databaseId;
        private bool _initialized = false;
        public DocumentStoreBase(string databaseId, string collectionId)
        {
            _databaseId = databaseId;
            _collectionId = collectionId;
        }

        public virtual async Task<bool> Init()
        {
            _documentClient = new DocumentClient(new Uri(Configs.Endpoint), Configs.Key);

            await _documentClient.CreateDatabaseIfNotExistsAsync(_databaseId).ConfigureAwait(false);
            await _documentClient.CreateCollectionIfNotExistsAsync(_databaseId, _collectionId).ConfigureAwait(false);
            

            _initialized = true;
            return _initialized;
        }

        protected virtual bool Initialized() => _initialized;
        protected virtual DocumentClient Client { get => this._documentClient; }

        protected virtual string Database { get => this._databaseId; }

        protected virtual string Collection { get => this._collectionId; }


        public virtual async Task<TDocument> GetAsync<TDocument>(string id)
        {
            try
            {
                var document = await _documentClient.ReadDocumentAsync
                    (UriFactory.CreateDocumentUri(_databaseId, _collectionId, id))
                    .ConfigureAwait(false);
                return (TDocument)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return default(TDocument);
                }
                else
                {
                    throw;
                }
            }
        }

        public virtual async Task<IEnumerable<TDocument>> QueryAsync<TDocument>
            (Expression<Func<TDocument, bool>> predicate)
        {
            var query = _documentClient.CreateDocumentQuery<TDocument>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();
            
            var results = new List<TDocument>();
            while (query.HasMoreResults)
            {
                results.AddRange((await query.ExecuteNextAsync<TDocument>().ConfigureAwait(false)));
            }
            return results;
        }

        public virtual async Task<Document> CreateAsync<TDocument>(TDocument document)
        {
            return await _documentClient.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), document)
                .ConfigureAwait(false);
        }

        public virtual async Task<Document> ReplaceAsync<TDocument>(string id, TDocument document)
        {
            return await _documentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), document)
                .ConfigureAwait(false);
        }

        public virtual async Task DeleteAsync(string id)
        {
            await _documentClient.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(_databaseId, _collectionId, id))
                .ConfigureAwait(false);
        }
    }
}
