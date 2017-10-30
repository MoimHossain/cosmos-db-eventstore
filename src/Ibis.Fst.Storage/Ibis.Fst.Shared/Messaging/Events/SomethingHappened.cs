using System;
using System.Collections.Generic;
using System.Text;

namespace Ibis.Fst.Shared.Messaging.Events
{
    public class SomethingHappened : EventBase
    {
        public SomethingHappened()
        {

        }

        public string EventDescription { get; set; }
    }
}
