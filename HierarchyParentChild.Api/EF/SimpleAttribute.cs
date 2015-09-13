using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HierarchyParentChild.Api.EF
{
    public partial class SimpleAttribute
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SimpleAttribute()
        {
            IncidentDatas = new HashSet<IncidentData>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(10)]
        public string Restriction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncidentData> IncidentDatas { get; set; }
    }
}
