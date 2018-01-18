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
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int transactionNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string itemCode { get; set; }

        public int adjustedQty { get; set; }

        public int balanceQty { get; set; }

        [StringLength(200)]
        public string remarks { get; set; }

        public virtual Stationery Stationery { get; set; }

        public virtual Transaction_Record Transaction_Record { get; set; }
    }
}
