using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Inventory_mvc.ViewModel
{
    public class UserViewModel
    {

        [Key]
        [Required]
        [StringLength(50)]
        [Display(Name = "User ID")]
        public string userID { get; set; }



        [Required]
        [Display(Name = "Password")]
        public string password { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string name { get; set; }

        [Required]
        [Display(Name = "Contact Number")]
        public int contactNo { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string address { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string role { get; set; }

        [Required]
        [Display(Name = "Department Code")]
        public string departmentCode { get; set; }

        [Required]
        [Display(Name = "User Email")]
        public string userEmail { get; set; }

        [Required]
        [Display(Name = "Delegation Start Date")]
        public DateTime delegatioStart { get; set; }

        [Required]
        [Display(Name = "Delegation End Date")]
        public DateTime delegationEnd { get; set; }


    }
}