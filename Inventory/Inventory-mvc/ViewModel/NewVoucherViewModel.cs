using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class NewVoucherViewModel
    {
        [Required]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        [Required]
        [Display(Name = "Quantity Adjusted")]
        public int Quantity { get; set; }

        public string Reason { get; set; }

        public string Description { get; set; }

        public string UOM { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.00}")]
        [Display(Name = "Unit Price")]
        public decimal Price { get; set; }
    }
}