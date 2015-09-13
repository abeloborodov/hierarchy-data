using System;

namespace HierarchyParentChild.Api.EF
{
    public partial class IncidentData
    {
        public Guid Id { get; set; }

        public Guid IncidentId { get; set; }

        public string Value { get; set; }

        public Guid AttributeMetadateId { get; set; }

        public Guid? SimpleAttributeId { get; set; }

        public virtual AttributeMetadata AttributeMetadata { get; set; }

        public virtual Incident Incident { get; set; }

        public virtual SimpleAttribute SimpleAttribute { get; set; }
    }
}
