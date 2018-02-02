using Inventory_mvc.Models;
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
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Reorder Level")]
        public int ReorderLevel { get; set; }

        [Required]
        [Display(Name = "Reorder Quantity")]
        public int ReorderQty { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "UOM")]
        public string UnitOfMeasure { get; set; }

        [Required]
        [Display(Name = "Stock Quantity")]
        public int StockQty { get; set; }

        [Required]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Supplier")]
        public string FirstSupplierCode { get; set; }

       
        [StringLength(50)]
        [Display(Name = "Second Supplier")]
        public string SecondSupplierCode { get; set; }

        
        [StringLength(50)]
        [Display(Name = "Third Supplier")]
        public string ThirdSupplierCode { get; set; }

        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Suggested")]
        public int Suggested { get; set; }

        [Display(Name = "Unfulfilled Qty")]
        public int unFulfilledQty { get; set; }

        [Display(Name = "Pending Restock Qty")]
        public int pendingRestockQty { get; set; }

        public virtual Category Category { get; set; }
    }
}