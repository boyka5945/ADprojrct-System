namespace Inventory_mvc.Models
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
            User2 = new HashSet<User>();
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

        [Required]
        [StringLength(50)]
        public string departmentHeadID { get; set; }

        public int collectionPointID { get; set; }

        [Required]
        [StringLength(50)]
        public string representativeID { get; set; }

        public virtual Collection_Point Collection_Point { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Record> Requisition_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> User2 { get; set; }
    }
}
