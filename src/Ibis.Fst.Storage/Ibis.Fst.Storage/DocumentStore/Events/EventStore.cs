using Ibis.Fst.Shared.Messaging;
using Ibis.Fst.Shared.Messaging.Events;
using Ibis.Fst.Storage.Supports;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ibis.Fst.Storage.DocumentStore.Events
{
    public class EventStore : DocumentStoreBase
    {
        public class StreamSchema
        {
            public string StreamHeadDocID { get; set; }
            public string PartitionColumnName { get; set; }
            public string PartitionValue { get; set; }
        }

        public EventStore(string databaseId, string collectionId) 
            : base(databaseId, collectionId)
        {

        }

        public async Task<bool> EmitEvents(StreamSchema schema, List<EisenAdded> events)
        {
            var spLink = UriFactory.CreateStoredProcedureUri(Database, Collection, 
                Constants.StoredProcedures.EmitEventsWithTransaction);
            var response = await Client.ExecuteStoredProcedureAsync<bool>
                    (spLink, new RequestOptions { PartitionKey = new PartitionKey(schema.PartitionValue) },
                    schema.StreamHeadDocID, schema.PartitionColumnName, schema.PartitionValue, events);

            return response.Response && response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}
