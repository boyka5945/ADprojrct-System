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
            Users = new HashSet<User>();
        }

        [Key]
        [Required]
        [Display(Name ="Department Code")]
        [StringLength(50)]
        public string departmentCode { get; set; }

        [Required]
        [Display(Name = "Department Name")]
        [StringLength(50)]
        public string departmentName { get; set; }

        [Required]
        [Display(Name = "Contact Name")]
        [StringLength(50)]
        public string contactName { get; set; }

        [Display(Name = "Phone Number")]
        public int phoneNo { get; set; }


        [Display(Name = "Fax Number")]
        public int faxNo { get; set; }

        [Display(Name = "Collection Point ID")]
        public int collectionPointID { get; set; }

        [Display(Name = "Collection Point")]
        public virtual Collection_Point Collection_Point { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Record> Requisition_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}
