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
        
        public string EventDescription { get; set; }
    }

    public class EisenUpdated : EventBase
    {
        public EisenUpdated()
        {

        }

        [JsonProperty(PropertyName = "project")]
        public string Project { get; set; }
        public string EventDescription { get; set; }
    }
}
