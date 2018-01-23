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
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        public string Description { get; set; }

        public string UOM { get; set; }

        [Display(Name = "Requisition Form No.")]
        public int RequisitionNo { get; set; }

        [Display(Name = "Request Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime RequestDate { get; set; }

        [Display(Name = "Request Quantity")]
        [Range(1,9999)]
        public int RequestQty { get; set; }

        [Display(Name = "Received Quantity")]
        public int ReceivedQty { get; set; }

    }
}