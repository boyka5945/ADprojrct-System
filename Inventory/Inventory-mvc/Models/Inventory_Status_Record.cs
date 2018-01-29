namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Inventory Status Record")]
    public partial class Inventory_Status_Record
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        [Display(Name = "Item Code")]
        public string itemCode { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        [Display(Name = "Date")]
        public DateTime date { get; set; }

        [Display(Name = "Available Quantity")]
        public int onHandQty { get; set; }

        [Display(Name = "Discrepancy Quantity")]
        public int discrepancyQty { get; set; }

        [StringLength(100)]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        public virtual Stationery Stationery { get; set; }
    }
}
