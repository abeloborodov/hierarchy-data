using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HierarchyParentChild.Api.EF
{
    public partial class Subservice
    {
        public Guid Id { get; set; }

        [Required]
        public string Namespace { get; set; }


        public virtual ICollection<SubserviceVersion> SubserviceVersions { get; set; }
    }
}
