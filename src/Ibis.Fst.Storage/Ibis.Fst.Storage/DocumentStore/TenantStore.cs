using Ibis.Fst.Shared.Models;
using Ibis.Fst.Shared.Supports;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ibis.Fst.Storage.DocumentStore
{
    public class TenantStore : DocumentStoreBase
    {
        public TenantStore(string databaseId, string collectionId) 
            : base(databaseId, collectionId)
        {

        }

        private KeysPair GetKeys(Tenant tenant)
        {
            if (tenant != null)
            {
                return new KeysPair(
                    tenant.TenantId.ToSafeStorageKey(),
                    tenant.TenantId.ToSafeStorageKey());
            }
            return default(KeysPair);
        }

        public async Task<Tenant> GetByNameAsync(string name)
        {
            var tenants = await base.QueryAsync<Tenant>((tenant) => tenant.Name.Equals(name)).ConfigureAwait(false);

            return tenants.FirstOrDefault();
        }

       
        public async Task SaveAsync(Tenant tenant)
        {
            Ensure.ArgumentNotNull(tenant, nameof(tenant));

            await base.CreateAsync<Tenant>(tenant).ConfigureAwait(false);
        }
    }
}
