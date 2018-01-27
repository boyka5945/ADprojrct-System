using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
namespace Inventory_mvc.ViewModel
{
    public class ReportViewModel
    {
        public Stationery Stationery { get; set; }

        public string ItemCode { get; set; }

        public string CategoryName { get; set; }

        public int Qty { get; set; }

        public decimal Cost { get; set; }


    }
}