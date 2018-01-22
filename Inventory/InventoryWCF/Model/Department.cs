namespace InventoryWCF.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Department")]
    public partial class Department
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Department()
        {
            Requisition_Record = new HashSet<Requisition_Record>();
            User = new HashSet<User>();
        }

        [Key]
        [StringLength(50)]
        public string departmentCode { get; set; }

        [Required]
        [StringLength(50)]
        public string departmentName { get; set; }

        [Required]
        [StringLength(50)]
        public string contactName { get; set; }

        public int phoneNo { get; set; }

        public int faxNo { get; set; }

        public int collectionPointID { get; set; }

        public virtual Collection_Point Collection_Point { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Record> Requisition_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> User { get; set; }
    }
}
