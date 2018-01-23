using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class Disbursement
    {
        public string itemDescription { get; set; }
        public string itemCode { get; set; }
        public int? quantity { get; set; }
    }
}