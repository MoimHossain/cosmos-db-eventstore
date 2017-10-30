using System;
using System.Collections.Generic;
using System.Text;

namespace Ibis.Fst.Shared.Messaging
{
    public abstract class MessageBase
    {
        public string id { get; set; }
        public string _ts { get; set; }
        public string _self { get; set; }
        public string _etag { get; set; }
        public string _rid { get; set; }
    }
}
