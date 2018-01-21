namespace InventoryWCF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("permissionInfo")]
    public partial class permissionInfo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public permissionInfo()
        {
            rolePermission = new HashSet<rolePermission>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int permissionID { get; set; }

        [StringLength(50)]
        public string controller { get; set; }

        [StringLength(50)]
        public string action { get; set; }

        [StringLength(50)]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rolePermission> rolePermission { get; set; }
    }
}
