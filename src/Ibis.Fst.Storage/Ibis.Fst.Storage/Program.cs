using Ibis.Fst.Shared.Messaging;
using Ibis.Fst.Shared.Messaging.Events;
using Ibis.Fst.Shared.Models;
using Ibis.Fst.Storage.DocumentStore;
using Ibis.Fst.Storage.DocumentStore.Events;
using Ibis.Fst.Storage.Supports;
using System;
using System.Collections.Generic;

namespace Ibis.Fst.Storage
{
    class Program
    {
        static void Main(string[] args)
        {
            //Sample1();

            EventSample();

            Console.WriteLine("Hello World!");
            //Console.ReadLine();
        }

        private static void EventSample()
        {
            var projectEventStore = DocumentStoreFactory.CreateAsync<EventStore>
                            (Constants.Databases.TenantDatabase("xyztenant"), 
                            Constants.Collections.EisenEventStore).Result;

            

            projectEventStore.Init().Wait();

            var schema = new EventStore.StreamSchema {
                PartitionColumnName = "id",
                PartitionValue = "aggregate-head-1"
            };

            projectEventStore.EmitEvents(schema, new List <EisenAdded> {
                new EisenAdded
                {
                    EventDescription = "Event1" + DateTime.UtcNow.Ticks.ToString()
                },
                new EisenAdded
                {
                 
                    EventDescription = "Event3" + DateTime.UtcNow.Ticks.ToString()
                }
            }).Wait();

        }

        private static void Sample1()
        {
            var tenantStore = DocumentStoreFactory.CreateAsync<TenantStore>
                            (Constants.Databases.TenantDatastore, Constants.Collections.TenantCollection).Result;

            var payload = new Tenant
            {
                Name = "ABC"
            };
            tenantStore.SaveAsync(payload).Wait();
            var tc = tenantStore.GetByNameAsync(payload.Name).Result;
        }
    }
}
