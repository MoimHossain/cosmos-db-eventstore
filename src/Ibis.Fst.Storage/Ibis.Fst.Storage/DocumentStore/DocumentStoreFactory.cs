using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ibis.Fst.Storage.DocumentStore
{
    public class DocumentStoreFactory
    {
        public static async Task<TDocumentStore> CreateAsync<TDocumentStore>(
            string databaseId, string collectionId)
            where TDocumentStore : DocumentStoreBase
        {
            var store = (DocumentStoreBase)Activator.CreateInstance
                (typeof(TDocumentStore), databaseId, collectionId);

            if (!(await store.Init().ConfigureAwait(false)))
            {
                // throw exception here? 
            }
            return (TDocumentStore)store;
        }
    }
}
