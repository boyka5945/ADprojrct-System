namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Requisition Details")]
    public partial class Requisition_Details
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int requisitionNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string itemCode { get; set; }

        [StringLength(100)]
        public string remarks { get; set; }

        public int qty { get; set; }

        public int? fulfilledQty { get; set; }

        [StringLength(50)]
        public string clerkID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? retrievedDate { get; set; }

        public int? allocatedQty { get; set; }

        [Column(TypeName = "date")]
        public DateTime? nextCollectionDate { get; set; }

        public virtual Requisition_Record Requisition_Record { get; set; }

        public virtual Stationery Stationery { get; set; }
    }
}
