namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Requisition Record")]
    public partial class Requisition_Record : IComparable
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Requisition_Record()
        {
            Requisition_Detail = new HashSet<Requisition_Detail>();
        }

        [Key]
        [Display(Name = "Requisition Number")]
        public int requisitionNo { get; set; }

        [Required]
        [Display(Name = "Department Code")]
        [StringLength(50)]
        public string deptCode { get; set; }

        [Required]
        [Display(Name = "Requester ID")]
        [StringLength(50)]
        public string requesterID { get; set; }

        [StringLength(50)]
        [Display(Name = "Approving Staff ID")]
        public string approvingStaffID { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Approve Date")]
        public DateTime? approveDate { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Status")]
        public string status { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Request Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? requestDate { get; set; }

        public virtual Department Department { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Requisition_Detail> Requisition_Detail { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }

        public int CompareTo(Object obj)
        {
            var rr = (Requisition_Record)obj;
            
            return requestDate.Value.CompareTo(rr.requestDate);
        }
    }
}
