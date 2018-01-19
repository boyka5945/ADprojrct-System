using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Inventory_mvc.ViewModel
{
    public class Purchase_Order_RecordViewModel
    {

        [Key]
        [Required]
        [Display(Name = "Order Number")]
        public int orderNo { get; set; }

        [Required]
        [Display(Name = "Supplier Code")]
        public string supplierCode { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime date { get; set; }

        [Required]
        [Display(Name = "Clerk ID")]
        public string clerkID { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string status { get; set; }

        [Required]
        [Display(Name = "Expected Delivery Date")]
        public DateTime expectedDeliveryDate { get; set; }
    }
}