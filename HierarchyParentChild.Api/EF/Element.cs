using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HierarchyParentChild.Api.EF
{
    public partial class Element
    {
        public Element()
        {
            AttributeMetadatas = new List<AttributeMetadata>();
            Parents = new List<TreeElement>();
            Children = new List<TreeElement>();
        }
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string EnglishName { get; set; }

        [Required]
        [StringLength(100)]
        public string RussianName { get; set; }

        public bool IsChoice { get; set; }
        public int Order { get; set; }


        public virtual ICollection<AttributeMetadata> AttributeMetadatas { get; set; }


        public Guid SubserviceVersionId { get; set; }
        public virtual SubserviceVersion SubserviceVersion { get; set; }
        

        public virtual ICollection<TreeElement> Parents { get; set; }
        public virtual ICollection<TreeElement> Children { get; set; }
    }

    public class TreeElement
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public virtual Element Parent { get; set; }

        public Guid ChildId { get; set; }
        public virtual Element Child { get; set; }
    }
}
