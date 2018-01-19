using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Inventory_mvc.Models;
using System.ComponentModel.DataAnnotations;

namespace Inventory_mvc.ViewModel
{
    public class RequisitionDetailViewModel
    {
        public string ItemCode { get; set; }

        public string Description { get; set; }

        public string UOM { get; set; }

        public int RequisitionNo { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime RequestDate { get; set; }

        [Range(1,9999)]
        public int RequestQty { get; set; }

        public int ReceivedQty { get; set; }

    }
}