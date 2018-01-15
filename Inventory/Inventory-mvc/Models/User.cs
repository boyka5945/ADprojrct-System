namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Adjustment_Voucher_Record = new HashSet<Adjustment_Voucher_Record>();
            Adjustment_Voucher_Record1 = new HashSet<Adjustment_Voucher_Record>();
            Department = new HashSet<Department>();
            Department1 = new HashSet<Department>();
            Purchase_Order_Record = new HashSet<Purchase_Order_Record>();
            Requisition_Record = new HashSet<Requisition_Record>();
            Requisition_Record1 = new HashSet<Requisition_Record>();
            Transaction_Record = new HashSet<Transaction_Record>();
        }

        [StringLength(50)]
        public string userID { get; set; }

        [Required]
        [StringLength(50)]
        public string password { get; set; }

        [Required]
        [StringLength(50)]
        public string name { get; set; }

        public int contactNo { get; set; }

        [Required]
        [StringLength(50)]
        public string address { get; set; }

        [Required]
        [StringLength(50)]
        public string role { get; set; }

        [Required]
        [StringLength(50)]
        public string departmentCode { get; set; }

        [Required]
        [StringLength(50)]
        public string userEmail { get; set; }

        [Column(TypeName = "date")]
        public DateTime? delegationStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime? delegationEnd { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adjustment_Voucher_Record> Adjustment_Voucher_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adjustment_Voucher_Record> Adjustment_Voucher_Record1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Department> Department1 { get; set; }

        public virtual Department Department2 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase_Order_Record> Purchase_Order_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Record> Requisition_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Record> Requisition_Record1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction_Record> Transaction_Record { get; set; }
    }
}
