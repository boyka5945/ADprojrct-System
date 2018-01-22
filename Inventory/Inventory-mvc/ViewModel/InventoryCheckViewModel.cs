using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class InventoryCheckViewModel
    {
        // STATIONERY INFORMATION
        [Required]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Description")]
        public string StationeryDescription { get; set; }

        public string UOM { get; set; }

        public string Location { get; set; }

        [Display(Name = "Category")]
        public string CategoryName { get; set; }

        public int CategoryID { get; set; }

        // QUANTITY RECORD
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [Required]
        [Display(Name = "Actual Quantity")]
        public int ActualQuantity { get; set; }
    }
}