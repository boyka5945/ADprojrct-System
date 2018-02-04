using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class RetrieveForm
    {
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "Quantity")]
        public int? Qty { get; set; }

        [Display(Name = "Stock Quantity")]
        public int? StockQty { get; set; }


        [Display(Name = "Retrieved Quantity")]
        public int? retrieveQty { get; set; }

        //[Display(Name = "allocated Quantity")]
        //public int? allocatedQty { get; set; }

    }
}