using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class RetrieveForm
    {
        public string ItemCode { get; set; }
        public string description { get; set; }
        public int? Qty { get; set; }
        public int? StockQty { get; set; }
        public int? retrieveQty { get; set; }
    }
}