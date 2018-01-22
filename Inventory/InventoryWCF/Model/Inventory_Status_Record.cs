namespace InventoryWCF.Model
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
        public string itemCode { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime date { get; set; }

        public int onHandQty { get; set; }

        public int discrepancyQty { get; set; }

        [StringLength(100)]
        public string remarks { get; set; }

        public virtual Stationery Stationery { get; set; }
    }
}
