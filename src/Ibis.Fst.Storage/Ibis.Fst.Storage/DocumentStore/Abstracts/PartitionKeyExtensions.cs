
using Ibis.Fst.Shared.Models;
using Ibis.Fst.Shared.Supports;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ibis.Fst.Storage.DocumentStore
{
    public static class PartitionKeyExtensions
    {
        public static string GetPartitionKey(DateTime dt)
        {
            return string.Format("{0}-{1}-{2}", dt.Year, dt.Month, dt.Day);
        }

        public static string ToSafeStorageKey(this Guid id)
        {
            return id.ToString("N").Replace("-", string.Empty);
        }

        public static KeysPair GetKeyPair(this Tenant tenant)
        {
            Ensure.ArgumentNotNull(tenant, nameof(tenant));

            return new KeysPair(
                tenant.TenantId.ToSafeStorageKey(),
                tenant.TenantId.ToSafeStorageKey());
        }
    }
}
