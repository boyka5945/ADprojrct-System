using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class AdjustmentVoucherViewModel
    {
        // STATIONERY INFORMATION
        [Required]
        [Display(Name = "Item Code")]
        public string ItemCode { get; set; }

        public string StationeryDescription { get; set; }

        public string UOM { get; set; }

        [DisplayFormat(DataFormatString = "{0:#.00}")]
        [Display(Name = "Unit Price")]
        public decimal Price { get; set; }


        // VOUCHER DETAIL INFORMATION
        [Required]
        [Display(Name = "Quantity Adjusted")]
        public int Quantity { get; set; }

        public string Reason { get; set; }

        // VOUCHER RECORD INFORMATION
        public int VoucherNo { get; set; }

        public decimal VoucherTotalAmount { get; set; }

        public string Requester { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime IssueDate { get; set; }

        public string Approver { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovalDate { get; set; }

        public string Causes { get; set; }

        public string Status { get; set; }

    }
}