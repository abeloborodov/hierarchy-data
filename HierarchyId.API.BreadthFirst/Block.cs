using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HierarchyId.API.BreadthFirst
{
    using HierarchyId = System.Data.Entity.Hierarchy.HierarchyId;

    public class Block
    {
        public Guid Id { get; set; }

        [Required]
        public string BlockName { get; set; }
        [Required]
        [Index("IX_BreadthFirst", 2, IsUnique = true)]
        public HierarchyId Path { get; set; }
        [Required]

        public bool IsChoice { get; set; }
        [Required]
        public int Order { get; set; }

        [Required]
        [Index("IX_BreadthFirst", 1, IsUnique = true)]
        public short Level { get; set; }

        public virtual ICollection<AttributeMetadata> AttributeList { get; set; }
    }
}