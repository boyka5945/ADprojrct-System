using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class Disbursement
    {


        [Display(Name = "Item Description")]
        public string itemDescription { get; set; }

        [Display(Name = "Item Code")]
        public string itemCode { get; set; }

        [Display(Name = "Quantity")]
        public int? quantity { get; set; }

        [Display(Name = "Department Code")]
        public string departmentCode { get; set; }

        [Display(Name = "Actual Quantity")]
        public int? actualQty { get; set; }
    }
}