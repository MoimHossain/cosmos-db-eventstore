using System;
using System.Collections.Generic;
using System.Text;

namespace Ibis.Fst.Shared.Models
{
    public class Tenant : ModelBase
    {        
        public Guid TenantId
        {
            get
            {
                return string.IsNullOrWhiteSpace(id) ? Guid.Empty : Guid.Parse(id);
            }
        }
        public string Name { get; set; }
    }
}
