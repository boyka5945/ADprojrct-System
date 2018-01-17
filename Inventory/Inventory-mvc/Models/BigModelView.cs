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
 
        public string itemCode { get; set; }
        public string description { get; set; }
        public string retrievedQuantity { get; set; }
        public int unfulfilledQty { get; set; }
        public int allocateQty { get; set; }
        public Requisition_Record requisitionRecord{ get; set; }

    }
}