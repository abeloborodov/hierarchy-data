using System;
using System.Collections.Generic;

namespace HierarchyId.API
{
    public class Subservice
    {
        public Guid Id { get; set; }
        public string Namespace { get; set; }

        public virtual ICollection<SubserviceVersion> SubserviceVersions { get; set; }

    }
}