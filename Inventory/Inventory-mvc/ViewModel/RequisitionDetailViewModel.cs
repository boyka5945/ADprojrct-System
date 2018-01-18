using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;

namespace Inventory_mvc.ViewModel
{
    public class RequisitionDetailViewModel
    {
        public Stationery Item { get; set; }

        public Requisition_Record Record { get; set; }

        public int RequestQty { get; set; }

        public int ReceivedQty { get; set; }
    }
}