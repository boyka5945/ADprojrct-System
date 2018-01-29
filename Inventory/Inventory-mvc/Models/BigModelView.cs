using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Inventory_mvc.Models
{
    public class BigModelView
    {

        [Display(Name = "Item Code")]
        public string itemCode { get; set; }

        [Display(Name = "Description")]
        public string description { get; set; }

        [Display(Name = "RetrievedQuantity")]
        public string retrievedQuantity { get; set; }

        [Display(Name = "Unfulfilled Quantity")]
        public int? unfulfilledQty { get; set; }

        [Display(Name = "Allocated Quantity")]
        public int? allocateQty { get; set; }

        [Display(Name = "Requisition Record")]
        public Requisition_Record requisitionRecord{ get; set; }

    }
}