using Ibis.Fst.Shared.Messaging.Events;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ibis.Fst.Storage.DocumentStore.Events
{
    public class EventStore : DocumentStoreBase
    {
        public EventStore(string databaseId, string collectionId) 
            : base(databaseId, collectionId)
        {

        }

        public async Task<bool> EmitEvents(List<SomethingHappened> events)
        {
            var spLink = UriFactory.CreateStoredProcedureUri(Database, Collection, "transaction-aware-event-emitter-proc");
            try
            {
                var response = await Client.ExecuteStoredProcedureAsync<SomethingHappened>(spLink, "prefix", events);

                return response.StatusCode == System.Net.HttpStatusCode.Accepted;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
