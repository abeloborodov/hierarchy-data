using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HierarchyParentChild.Api.EF
{
    [Table("Incident")]
    public partial class Incident
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Incident()
        {
            IncidentDatas = new HashSet<IncidentData>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string TicketNumber { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncidentData> IncidentDatas { get; set; }
    }
}
