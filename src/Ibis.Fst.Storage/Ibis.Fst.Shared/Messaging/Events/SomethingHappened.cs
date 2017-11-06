using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ibis.Fst.Shared.Messaging.Events
{
    public class EisenAdded : EventBase
    {
        public EisenAdded()
        {

        }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "project")]
        public string Project { get; set; }
        public string EventDescription { get; set; }
    }

    public class EisenUpdated : EventBase
    {
        public EisenUpdated()
        {

        }

        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }

        [JsonProperty(PropertyName = "project")]
        public string Project { get; set; }
        public string EventDescription { get; set; }
    }
}
