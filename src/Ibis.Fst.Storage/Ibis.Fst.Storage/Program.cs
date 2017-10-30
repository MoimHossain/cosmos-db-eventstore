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
            Console.ReadLine();
        }

        private static void EventSample()
        {
            var projectEventStore = DocumentStoreFactory.CreateAsync<EventStore>
                            (Constatnts.Databases.TenantDatabase("xyztenant"), 
                            Constatnts.Collections.ProjectEventStore).Result;

            projectEventStore.Init().Wait();

            projectEventStore.EmitEvents(new List<SomethingHappened> {
                new SomethingHappened
                {
                    EventDescription = "Event1" + DateTime.UtcNow.Ticks.ToString()
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
