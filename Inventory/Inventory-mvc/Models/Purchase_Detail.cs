namespace Inventory_mvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Purchase Details")]
    public partial class Purchase_Detail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int orderNo { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        //[Required]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "itemCode can not be empty.")]
        public string itemCode { get; set; }

        [StringLength(100)]
        public string remarks { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "qty can not be empty.")]
        public int qty { get; set; }

        public int? fulfilledQty { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "price can not be empty.")]
        public decimal price { get; set; }

        public int? deliveryOrderNo { get; set; }

        public virtual Purchase_Order_Record Purchase_Order_Record { get; set; }

        public virtual Stationery Stationery { get; set; }
    }
}
