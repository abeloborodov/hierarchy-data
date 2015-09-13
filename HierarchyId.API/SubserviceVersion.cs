using System;
using System.ComponentModel.DataAnnotations;

namespace HierarchyId.API
{
    public class SubserviceVersion
    {
        public Guid Id { get; set; }

        public Guid SubserviceId { get; set; }

        [Required]
        [StringLength(5)]
        public string Version { get; set; }

        public virtual Subservice Subservice { get; set; }


    }
}