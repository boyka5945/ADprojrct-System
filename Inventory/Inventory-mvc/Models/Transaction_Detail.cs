namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transaction Details")]
    public partial class Transaction_Detail
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Transaction Number")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int transactionNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        [Display(Name = "Item Code")]
        public string itemCode { get; set; }

        [Display(Name = "Adjusted Quantity")]
        public int adjustedQty { get; set; }

        [Display(Name = "Balance Quantity")]
        public int balanceQty { get; set; }

        [StringLength(200)]
        [Display(Name = "Remarks")]
        public string remarks { get; set; }

        public virtual Stationery Stationery { get; set; }

        [Display(Name = "Transaction Record")]
        public virtual Transaction_Record Transaction_Record { get; set; }
    }
}
