using System;
using System.Collections.Generic;
using System.Text;

namespace Ibis.Fst.Storage.Supports
{
    public static class Constants
    {
        public static class Databases
        {
            public static string TenantCatalogDb = "tenant-catalog";
            public static Func<string, string> TenantDb = (tenantId) => string.Format("{0}-db", tenantId);
        }

        public static class Collections
        {
            public static string TenantCatalog= "tenants";

            public static string ProjectEventStore = "gevents";
        }

        public static class StoredProcedures
        {
            public static string EmitEventsWithTransaction = "transaction-aware-event-emitter-proc";
        }
    }
}
