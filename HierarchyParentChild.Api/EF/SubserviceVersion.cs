using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HierarchyParentChild.Api.EF
{
    public partial class SubserviceVersion
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(5)]
        public string Version { get; set; }


        public Guid SubserviceId { get; set; }
        public virtual Subservice Subservice { get; set; }
        public virtual ICollection<Element> Elements { get; set; }
    }
}
