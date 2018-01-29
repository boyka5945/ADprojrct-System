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

        [Display(Name = "Description")]
        public string StationeryDescription { get; set; }

        public string UOM { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Unit Price")]
        public decimal Price { get; set; }


        // VOUCHER DETAIL INFORMATION
        [Required]
        [Display(Name = "Quantity Adjusted")]
        public int Quantity { get; set; }


        public string Reason { get; set; }

        // VOUCHER RECORD INFORMATION
        [Display(Name = "Voucher Number")]
        public int VoucherNo { get; set; }

        [Display(Name = "Discrepancy Amount")]
        public decimal VoucherTotalAmount { get; set; }

        [Display(Name = "Issued By")]
        public string Requester { get; set; }

        [Display(Name = "Issue Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime IssueDate { get; set; }

        [Display(Name = "Processed By")]
        public string Approver { get; set; }

        [Display(Name = "Processed Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yy}", ApplyFormatInEditMode = true)]
        public DateTime? ApprovalDate { get; set; }

        public string Causes { get; set; }

        public string Status { get; set; }

    }
}