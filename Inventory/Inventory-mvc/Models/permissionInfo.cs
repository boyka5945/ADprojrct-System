namespace Inventory_mvc.Models
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
            rolePermissions = new HashSet<rolePermission>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Permission ID")]
        public int permissionID { get; set; }

        [StringLength(50)]
        [Display(Name = "Controller")]
        public string controller { get; set; }

        [StringLength(50)]
        [Display(Name = "Action")]
        public string action { get; set; }

        [StringLength(50)]
        [Display(Name = "Description")]
        public string description { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<rolePermission> rolePermissions { get; set; }
    }
}
