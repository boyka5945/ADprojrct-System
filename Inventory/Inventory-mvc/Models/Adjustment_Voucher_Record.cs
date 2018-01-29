namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Adjustment Voucher Record")]
    public partial class Adjustment_Voucher_Record
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Adjustment_Voucher_Record()
        {
            Voucher_Details = new HashSet<Voucher_Detail>();
        }

        [Key]
        [Display(Name = "Voucher ID")]
        public int voucherID { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Issue Date")]
        public DateTime issueDate { get; set; }

        [Required]
        [Display(Name = "Handling Staff ID")]
        [StringLength(50)]
        public string handlingStaffID { get; set; }

        [StringLength(50)]
        [Display(Name = "Authorising Staff ID")]
        public string authorisingStaffID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string status { get; set; }

        [StringLength(200)]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Approval Date")]
        public DateTime? approvalDate { get; set; }

        [Display(Name = "Requester")]
        public virtual User User { get; set; }

        [Display(Name = "Approver")]
        public virtual User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voucher_Detail> Voucher_Details { get; set; }
    }
}
