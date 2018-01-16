using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class StationeryViewModel
    {
        [Key]
        [Required]
        [StringLength(50)]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Required]
        [Display(Name = "Category ID")]
        public int CategoryID { get; set; }

        [Required]
        [Display(Name = "Reorder Level")]
        public int ReorderLevel { get; set; }

        [Required]
        [Display(Name = "Reorder Quantity")]
        public int ReorderQty { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Unit Of Measure")]
        public string UnitOfMeasure { get; set; }

        [Required]
        [Display(Name = "Stock Quantity")]
        public int StockQty { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Supplier Code")]
        public string FirstSupplierCode { get; set; }

        [Required]
        [Display(Name = "Price")]
        public double Price { get; set; }
    }
}