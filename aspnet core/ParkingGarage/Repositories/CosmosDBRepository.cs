using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using ParkingGarage.Common;

namespace ParkingGarage.Repositories {

    public class CosmosDBRepository<T> : ICosmosDBRepository<T> where T : class {
        private readonly string _endpoint;
        private readonly string _key;
        private readonly string _databaseId;
        private readonly string _collectionId;

        private readonly CosmosSettings _cosmosSettings;
        private static DocumentClient client;

        public CosmosDBRepository(IOptions<CosmosSettings> cosmosSettings) {
            _cosmosSettings = cosmosSettings.Value;
            _endpoint = _cosmosSettings.Endpoint;
            _key = _cosmosSettings.Key;
            _databaseId = _cosmosSettings.DatabaseId;
            _collectionId = _cosmosSettings.CollectionId;
        }

        public async Task<T> GetItemAsync(string id) {
            try {
                Document document = await client.ReadDocumentAsync(
                    UriFactory.CreateDocumentUri(_databaseId, _collectionId, id),
                    new RequestOptions { PartitionKey = new PartitionKey("Parking Garage") }
                );
                return (T)(dynamic)document;
            } catch (DocumentClientException e) {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    return null;
                } else {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate) {
            IDocumentQuery<T> query = client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults) {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<Document> CreateItemAsync(T item) {
            return await client.CreateDocumentAsync(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                item,
                new RequestOptions { PartitionKey = new PartitionKey("Parking Garage") }
            );
        }

        public async Task<Document> UpdateItemAsync(string id, T item) {
            return await client.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(_databaseId, _collectionId, id),
                item,
                new RequestOptions { PartitionKey = new PartitionKey("Parking Garage") });
        }

        public async Task DeleteItemAsync(string id) {
            await client.DeleteDocumentAsync(
                UriFactory.CreateDocumentUri(_databaseId, _collectionId, id),
                new RequestOptions { PartitionKey = new PartitionKey("Parking Garage") }
            );
        }

        public async Task Initialize() {
            client = new DocumentClient(new Uri(_endpoint), _key);
            await CreateDatabaseIfNotExistsAsync();
            await CreateCollectionIfNotExistsAsync();
        }

        private async Task CreateDatabaseIfNotExistsAsync() {
            try {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_databaseId));
            } catch (DocumentClientException e) {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    await client.CreateDatabaseAsync(new Database { Id = _databaseId });
                } else {
                    throw;
                }
            }
        }

        private async Task CreateCollectionIfNotExistsAsync() {
            try {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId));
            } catch (DocumentClientException e) {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound) {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(_databaseId),
                        new DocumentCollection { Id = _collectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                } else {
                    throw;
                }
            }
        }
    }
}
