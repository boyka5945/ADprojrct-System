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
            Purchase_Order_Record = new HashSet<Purchase_Order_Record>();
            Requisition_Record = new HashSet<Requisition_Record>();
            Requisition_Record1 = new HashSet<Requisition_Record>();
            Transaction_Record = new HashSet<Transaction_Record>();
        }

        [Required]
        [StringLength(50)]
        [Display(Name = "User ID")]
        public string userID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Name")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        public int contactNo { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int role { get; set; }

        //[Required]
        [StringLength(50)]
        [Display(Name = "Department Code")]
        public string departmentCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "User Email")]
        public string userEmail { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Delegation Start")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? delegationStart { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Delegation End")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? delegationEnd { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adjustment_Voucher_Record> Adjustment_Voucher_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Adjustment_Voucher_Record> Adjustment_Voucher_Record1 { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Purchase_Order_Record> Purchase_Order_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Record> Requisition_Record { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Record> Requisition_Record1 { get; set; }

        [Display(Name = "Role Info")]
        public virtual roleInfo roleInfo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Transaction_Record> Transaction_Record { get; set; }
    }
}
