using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HierarchyId.API
{
    using HierarchyId = System.Data.Entity.Hierarchy.HierarchyId;

    public class Block
    {
        public Guid Id { get; set; }

        [Required]
        public string BlockName { get; set; }
        [Required]
        public HierarchyId Path { get; set; }
        [Required]
        public bool IsChoice { get; set; }
        [Required]
        public int Order { get; set; }

        public virtual ICollection<AttributeMetadata> AttributeList { get; set; }
    }
}