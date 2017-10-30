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
                            (Constatnts.Databases.TenantDatabase("xyztenant"), 
                            Constatnts.Collections.EisenEventStore).Result;

            var projectID = Guid.Parse("{EDDB97B6-9EC1-48F6-8AB9-2AC57CB5F96B}").ToString("N");

            projectEventStore.Init().Wait();

            var schema = new EventStore.StreamSchema {
                PartitionColumnName = "project",
                PartitionValue = projectID
            };

            projectEventStore.EmitEvents(schema, new List <EisenAdded> {
                new EisenAdded
                {
                    project = projectID,
                    EventDescription = "Event1" + DateTime.UtcNow.Ticks.ToString()
                },
                new EisenAdded
                {
                    project = projectID,
                    EventDescription = "Event3" + DateTime.UtcNow.Ticks.ToString()
                }
            }).Wait();

        }

        private static void Sample1()
        {
            var tenantStore = DocumentStoreFactory.CreateAsync<TenantStore>
                            (Constatnts.Databases.TenantDatastore, Constatnts.Collections.TenantCollection).Result;

            var payload = new Tenant
            {
                Name = "ABC"
            };
            tenantStore.SaveAsync(payload).Wait();
            var tc = tenantStore.GetByNameAsync(payload.Name).Result;
        }
    }
}
