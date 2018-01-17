using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Inventory_mvc.ViewModel
{
    public class SupplierViewModel
    {
        [Key]
        [Required]
        [StringLength(50)]
        [Display(Name = "Supplier Code")]
        public string SupplierCode { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "GST Registration No.")]
        public string GSTNo { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Company Name")]
        public string SupplierName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [Required]
        [Display(Name = "Phone Number")]
        public int? PhoneNo { get; set; }

        [Display(Name = "Fax Number")]
        public int? FaxNo { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Address")]
        public string Address { get; set; }
    }
}