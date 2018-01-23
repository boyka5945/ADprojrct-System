using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class ItemTransactionRecordViewModel
    {
        [Display(Name = "Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime TransactionDate { get; set; }

        public int TransactionNo { get; set; }

        public string ItemCode { get; set; }

        public string TransactionType { get; set; }

        public string Remarks { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Balance")]
        public int BalanceQty { get; set; }
    }
}