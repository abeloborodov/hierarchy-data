using System;

namespace HierarchyId.API.BreadthFirst
{
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