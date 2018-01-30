namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Requisition Details")]
    public partial class Requisition_Detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name = "Requisition Number")]
        public int requisitionNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        [Display(Name = "Item Code")]
        public string itemCode { get; set; }

        [StringLength(200)]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Display(Name = "Quantity")]
        public int? qty { get; set; }

        [Display(Name = "Fulfilled Quantity")]
        public int? fulfilledQty { get; set; }

        [StringLength(50)]
        [Display(Name = "Clerk ID")]
        public string clerkID { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Retrieved Date")]
        public DateTime? retrievedDate { get; set; }

        [Display(Name = "Allocated Quantity")]
        public int? allocatedQty { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Next Collection Date")]
        public DateTime? nextCollectionDate { get; set; }

        [Display(Name = "Requisition Record")]
        public virtual Requisition_Record Requisition_Record { get; set; }

        public virtual Stationery Stationery { get; set; }
    }
}
