using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
namespace Inventory_mvc.ViewModel
{
    public class ReportViewModel
    {
         
        public string ItemCode { get; set; }

        public string ItemDescription { get; set; }

        public string CategoryName { get; set; }

        public int RequestQuantity { get; set; }

        public decimal Cost { get; set; }

        public string RequesterDepartment { get; set; }

        public string Supplier { get; set; }

        public int OrderQuantity { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public string Status { get; set; }
    }
}