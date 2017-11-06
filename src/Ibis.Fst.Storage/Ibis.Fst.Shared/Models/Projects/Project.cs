using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ibis.Fst.Shared.Models.Projects
{
    public class Project : ModelBase
    {
        public Guid ProjectId
        {
            get
            {
                return string.IsNullOrWhiteSpace(id) ? Guid.Empty : Guid.Parse(id);
            }
        }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
