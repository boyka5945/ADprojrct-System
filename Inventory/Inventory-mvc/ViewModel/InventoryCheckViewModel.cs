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

        [Display(Name = "Category ID")]
        public int CategoryID { get; set; }

        // QUANTITY RECORD
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [Required]
        [Display(Name = "Actual Quantity")]
        public int ActualQuantity { get; set; }

        public int Discrepancy { get; set; }

        public string Remarks { get; set; }

        [Display(Name = "Stock Check Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StockCheckDate { get; set; }
    }
}