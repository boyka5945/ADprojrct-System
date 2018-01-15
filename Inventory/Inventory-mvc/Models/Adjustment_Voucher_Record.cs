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
            Voucher_Details = new HashSet<Voucher_Details>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int voucherID { get; set; }

        [Column(TypeName = "date")]
        public DateTime issueDate { get; set; }

        [Required]
        [StringLength(50)]
        public string handlingStaffID { get; set; }

        [StringLength(50)]
        public string authorisingStaffID { get; set; }

        [Required]
        [StringLength(50)]
        public string status { get; set; }

        [StringLength(50)]
        public string remarks { get; set; }

        [Column(TypeName = "date")]
        public DateTime? approvalDate { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Voucher_Details> Voucher_Details { get; set; }
    }
}
