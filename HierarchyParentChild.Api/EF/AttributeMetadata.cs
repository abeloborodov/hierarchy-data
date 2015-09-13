using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HierarchyParentChild.Api.EF
{
    public partial class AttributeMetadata
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string Placeholder { get; set; }



        public int Order { get; set; }

        public Guid ElementId { get; set; }
        public virtual Element Element { get; set; }

        

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncidentData> IncidentDatas { get; set; }
    }
}
