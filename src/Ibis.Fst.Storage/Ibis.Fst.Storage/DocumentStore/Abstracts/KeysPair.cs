using System;
using System.Collections.Generic;
using System.Text;

namespace Ibis.Fst.Storage.DocumentStore
{
    public class KeysPair
    {
        public KeysPair(string partitionkey, string rowKey)
        {
            this.PartitionKey = partitionkey;
            this.RowKey = rowKey;
        }

        public string PartitionKey { get; private set; }
        public string RowKey { get; private set; }
    }
}
