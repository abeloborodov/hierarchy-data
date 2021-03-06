using System;

namespace HierarchyId.API
{
    using HierarchyId = System.Data.Entity.Hierarchy.HierarchyId;

    public class AttributeMetadata
    {
        public Guid Id { get; set; }
        public Guid BlockId { get; set; }

        public string AttributeName { get; set; }

        public string Placeholder { get; set; }

        public virtual Block Block { get; set; }

        public int Order { get; set; }
    }
}