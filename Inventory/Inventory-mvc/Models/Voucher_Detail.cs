namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Voucher Details")]
    public partial class Voucher_Detail
    {
        [Key]
        [Display(Name = "Voucher ID")]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int voucherID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        [Display(Name = "Item Code")]
        public string itemCode { get; set; }

        [Display(Name = "Adjusted Quantity")]
        public int adjustedQty { get; set; }

        [StringLength(200)]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        [Display(Name = "Adjustment Voucher Record")]
        public virtual Adjustment_Voucher_Record Adjustment_Voucher_Record { get; set; }

        public virtual Stationery Stationery { get; set; }
    }
}
